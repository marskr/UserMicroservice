using Microsoft.AspNetCore.Mvc;
using System;
using UsersMicroservice.Data;
using UsersMicroservice.Models;
using UsersMicroservice.Queries;

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
                Users specifiedUser = _query.APIGet(token_s, _context);

                if (specifiedUser == null || specifiedUser.AuthTokenExpiration < DateTime.Now) { return new ObjectResult(false); }

                return new ObjectResult(true);
            }
            catch
            {
                return BadRequest();
            }
        }

        // POST api/Authentication/{email}/{password}
        [HttpPost("{email_s}/{password_s}", Name = "CreateToken")]
        public IActionResult Post(string email_s, string password_s)
        {
            if (email_s == null || password_s == null) return BadRequest();

            return new ObjectResult(_query.APICreateToken(email_s, password_s));
        }

        // PUT api/Authentication/{token}
        [HttpPut("{token_s}", Name = "UpdateAuthExpirationDate")]
        public IActionResult Put(string token_s)
        {
            try
            {
                Users updatedUser = _query.APIGet(token_s, _context);
                if (updatedUser == null) { return new ObjectResult(false); }
                else
                {
                    _query.APIPut(updatedUser, null, _context);
                }
            }
            catch
            {
                return BadRequest();
            }

            return new ObjectResult(true);
        }
    }
}
