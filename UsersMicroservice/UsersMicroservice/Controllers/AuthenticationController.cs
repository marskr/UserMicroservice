using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using UsersMicroservice.Data;
using UsersMicroservice.Logs;
using UsersMicroservice.Models;
using UsersMicroservice.Queries;
using UsersMicroservice.Security;

namespace UsersMicroservice.Controllers
{
    [Route("api/[controller]")]
    public class AuthenticationController : Controller
    {
        AppDbContext _context;
        AbstractQueriesFactory<Users, AppDbContext> _query;

        public AuthenticationController(AppDbContext context)
        {
            _context = context;
            _query = new UserAuthQueriesFactory();
        }

        // GET api/Authentication/{token}
        [HttpGet("{token_s}", Name = "GetAuthResponse")]
        public IActionResult Get(string token_s)
        {
            try
            {
                Users specifiedUser = _query.APIGetByEmail(token_s, _context);

                if (specifiedUser == null || specifiedUser.AuthTokenExpiration < DateTime.Now)
                {
                    return new ObjectResult(ResponsesContainer.Instance.GetResponseContent(HttpStatusCode.OK,
                                            String.Empty, false, 2, "Validation error", "Provided token is not valid"));
                }
                return new ObjectResult(ResponsesContainer.Instance.GetResponseContent(HttpStatusCode.OK,
                                        String.Empty, true, 0, "Valid", "Provided token is valid"));
            }
            catch (Exception ex)
            {
                ErrInfLogger.LockInstance.ErrorLog(ex.ToString());
                return new ObjectResult(ResponsesContainer.Instance.GetResponseContent(HttpStatusCode.BadRequest,
                                        String.Empty, false, 4, "Exception", "Application exception thrown"));
            }
        }

        // POST api/Authentication/{email}/{password}
        [HttpPost("{email_s}/{password_s}", Name = "CreateToken")]
        public IActionResult Post(string email_s, string password_s)
        {
            if (XSS.CheckIfTooLong(email_s, 30))
                return new ObjectResult(ResponsesContainer.Instance.GetResponseContent(HttpStatusCode.OK,
                                        String.Empty, false, 7, "Bad email", "The email is too long"));

            if (XSS.CheckIfContains(email_s, XSS.forbiddenList_s))
                return new ObjectResult(ResponsesContainer.Instance.GetResponseContent(HttpStatusCode.OK,
                                        String.Empty, false, 8, "Bad email", "The email contains forbidden signs"));

            if (XSS.CheckIfTooLong(password_s, 50))
                return new ObjectResult(ResponsesContainer.Instance.GetResponseContent(HttpStatusCode.OK,
                                        String.Empty, false, 7, "Bad password", "The pasword is too long"));

            try
            {
                if (email_s == null || password_s == null)
                    return new ObjectResult(ResponsesContainer.Instance.GetResponseContent(
                                            HttpStatusCode.Unauthorized, String.Empty, false, 1, 
                                            "Authorization Error", "Wrong login or password"));

                string token_s = _query.APICreateToken(email_s, password_s, _context);

                if (token_s == null) return new ObjectResult(ResponsesContainer.Instance.GetResponseContent(
                                            HttpStatusCode.Unauthorized, String.Empty, false, 1,
                                            "Authorization Error", "Wrong login or password"));

                return new ObjectResult(ResponsesContainer.Instance.GetResponseContent(HttpStatusCode.OK,
                                        token_s, true, 0, "Authorized", "Password & login correct"));
            }
            catch (Exception ex)
            {
                ErrInfLogger.LockInstance.ErrorLog(ex.ToString());
                return new ObjectResult(ResponsesContainer.Instance.GetResponseContent(HttpStatusCode.BadRequest,
                                        String.Empty, false, 4, "Exception", "Application exception thrown"));
            }
        }
    }
}
