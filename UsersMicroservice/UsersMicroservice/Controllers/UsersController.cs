using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Net;
using UsersMicroservice.Data;
using UsersMicroservice.Logs;
using UsersMicroservice.Models;
using UsersMicroservice.Queries;

namespace UsersMicroservice.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        AppDbContext _context;
        AbstractQueriesFactory<Users, AppDbContext> _query;

        public UsersController(AppDbContext context)
        {
            _context = context;
            _query = new UserCRUDQueriesFactory();
        }

        // GET api/Users/{email}
        [HttpGet("{email_s}", Name = "GetSingleUser")]
        public IActionResult Get(string email_s)
        {
            Users specifiedUser = _query.APIGet(email_s, _context);
            if (specifiedUser == null)
                return new ObjectResult(ResponsesContainer.Instance.GetResponseContent(HttpStatusCode.OK,
                                        String.Empty, false, 3, "Invalid user", "User not found in database"));

            string specifiedUser_s = JsonConvert.SerializeObject(specifiedUser, Formatting.Indented);

            // need to change response body for USER!!!!
            return new ObjectResult(ResponsesContainer.Instance.GetResponseContent(HttpStatusCode.OK,
                                    specifiedUser_s, true, 0, "Found", "User found in database"));
        }

        // POST api/Users/
        [HttpPost(Name = "CreateUser")]
        public IActionResult Post(/*[FromBody]*/Users newUser)
        {
            if (_query.APIGet(newUser.Email, _context) != null)
                return new ObjectResult(ResponsesContainer.Instance.GetResponseContent(HttpStatusCode.OK,
                                        String.Empty, false, 5, "User exists", "User exists in database"));
            try
            {
                _query.APIPost(newUser, _context);
            }
            catch (Exception ex)
            {
                ErrInfLogger.LockInstance.ErrorLog(ex.ToString());
                return new ObjectResult(ResponsesContainer.Instance.GetResponseContent(HttpStatusCode.BadRequest,
                                        String.Empty, false, 4, "Exception", "Application exception thrown"));
            }
            return new ObjectResult(ResponsesContainer.Instance.GetResponseContent(HttpStatusCode.OK,
                                    String.Empty, true, 0, "Created", "User created in database"));
        }

        // PUT api/Users/{email}
        [HttpPut("{email_s}", Name = "UpdateUser")]
        public IActionResult Put(string email_s, /*[FromBody]*/Users newUser)
        {
            try
            {
                Users updatedUser = _query.APIGet(email_s, _context);
                if (updatedUser == null) { return new ObjectResult(ResponsesContainer.Instance.GetResponseContent(
                                                                   HttpStatusCode.OK, String.Empty, false, 3,
                                                                   "Invalid user", "User not found in database"));
                }
                else
                {
                    // in this moment we assume that email, permissionid, salt
                    // is UNALTERABLE!!
                    _query.APIPut(updatedUser, newUser, _context);
                }
            }
            catch (Exception ex)
            {
                ErrInfLogger.LockInstance.ErrorLog(ex.ToString());
                return new ObjectResult(ResponsesContainer.Instance.GetResponseContent(HttpStatusCode.BadRequest,
                                        String.Empty, false, 4, "Exception", "Application exception thrown"));
            }
            return new ObjectResult(ResponsesContainer.Instance.GetResponseContent(HttpStatusCode.OK,
                                    String.Empty, true, 0, "Updated", "User updated in database"));
        }

        // DELETE api/users/{email}
        [HttpDelete("{email_s}", Name = "DeleteUser")]
        public IActionResult Delete(string email_s)
        {
            try
            {
                Users deletedUser = _query.APIGet(email_s, _context);
                if (deletedUser == null) { return new ObjectResult(ResponsesContainer.Instance.GetResponseContent(
                                                                   HttpStatusCode.OK, String.Empty, false, 6, 
                                                                   "Not exists", "User not exists in database"));
                }

                _query.APIDelete(deletedUser, _context);
            }
            catch (Exception ex)
            {
                ErrInfLogger.LockInstance.ErrorLog(ex.ToString());
                return new ObjectResult(ResponsesContainer.Instance.GetResponseContent(HttpStatusCode.BadRequest,
                                        String.Empty, false, 4, "Exception", "Application exception thrown"));
            }
            return new ObjectResult(ResponsesContainer.Instance.GetResponseContent(HttpStatusCode.OK, String.Empty, 
                                                                                   true, 0, "Deleted", 
                                                                                   "User deleted from database"));
        }
    }
}
