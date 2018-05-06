﻿using Microsoft.AspNetCore.Mvc;
using System;
using UsersMicroservice.Data;
using UsersMicroservice.Logs;
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

                if (specifiedUser == null || specifiedUser.AuthTokenExpiration < DateTime.Now)
                {
                    return new ObjectResult(false);
                }
                return new ObjectResult(true);
            }
            catch (Exception ex)
            {
                ErrInfLogger.LockInstance.ErrorLog(ex.ToString());
                return BadRequest();
            }
        }

        // POST api/Authentication/{email}/{password}
        [HttpPost("{email_s}/{password_s}", Name = "CreateToken")]
        public IActionResult Post(string email_s, string password_s)
        {
            if (email_s == null || password_s == null) return BadRequest();

            string token_s = _query.APICreateToken(email_s, password_s, _context);

            if (token_s == null) return NotFound();
            return new ObjectResult(token_s);
        }
    }
}
