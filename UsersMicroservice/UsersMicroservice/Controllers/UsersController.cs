using Microsoft.AspNetCore.Mvc;
using UsersMicroservice.Data;
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
            _query = new UserQueriesFactory();
        }

        // GET api/users/{email_s}
        [HttpGet("{email_s}", Name = "GetSingleUser")]
        public IActionResult Get(string email_s)
        {
            Users specifiedUser = _query.APIGet(email_s, _context);

            if (specifiedUser == null) { return NotFound(); }

            return new ObjectResult(specifiedUser);
        }

        // POST api/users/
        [HttpPost]
        public IActionResult Post([FromBody]Users newUser)
        {
            // dopisz ID ktore ma zostac dodane automatycznie (max id + 1), losowanie soli, tokenow
            if (newUser == null) { return BadRequest(); }
            if (_query.APIGet(newUser.Email, _context) != null) { return NotFound(); }
            try
            {
                _query.APIPost(newUser, _context);
            }
            catch
            {
                return BadRequest();
            }
            
            return CreatedAtRoute("GetSingleUser", new { email_s = newUser.Email }, newUser);
        }

        // PUT api/users/5
        [HttpPut("{email_s}")]
        public IActionResult Put(string email_s, [FromBody]Users newUser)
        {
            try
            {
                Users updatedUser = _query.APIGet(email_s, _context);
                if (updatedUser == null) { return NotFound(); }
                else
                {
                    // in this moment we assume that id, email, salt, authtoken, authtokenexpiration & permissionid 
                    // is UNALTERABLE!!
                    _query.APIPut(updatedUser, newUser, _context);
                }
            }
            catch
            {
                return BadRequest();
            }

            return CreatedAtRoute("GetSingleUser", new { email_s = newUser.Email }, newUser);
        }

        // DELETE api/users/{email}
        [HttpDelete("{email_s}")]
        public IActionResult Delete(string email_s)
        {
            try
            {
                Users deletedUser = _query.APIGet(email_s, _context);
                if (deletedUser == null) { return NotFound(); }

                _query.APIDelete(deletedUser, _context);
            }
            catch
            {
                return BadRequest();
            }
            
            return new NoContentResult();
        }

        //// GET api/users/top/{number_i}
        //[HttpGet("top/{number_i}")]
        //public IActionResult GetTop(int number_i)
        //{
        //    IOrderedQueryable topUsers = _context.Users.Take(number_i).OrderBy(t => t.Email);
        //    if (topUsers == null) { return NotFound(); }

        //    return new ObjectResult(topUsers);
        //}
    }
}
