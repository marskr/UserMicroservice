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
            _query = new UserCRUDQueriesFactory();
        }

        // GET api/Users/{email}
        [HttpGet("{email_s}", Name = "GetSingleUser")]
        public IActionResult Get(string email_s)
        {
            Users specifiedUser = _query.APIGet(email_s, _context);
            if (specifiedUser == null) { return NotFound(); }

            return new ObjectResult(specifiedUser);
        }

        // POST api/Users/
        [HttpPost(Name = "CreateUser")]
        public IActionResult Post([FromBody]Users newUser)
        {
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

        // PUT api/Users/{email}
        [HttpPut("{email_s}", Name = "UpdateUser")]
        public IActionResult Put(string email_s, [FromBody]Users newUser)
        {
            try
            {
                Users updatedUser = _query.APIGet(email_s, _context);
                if (updatedUser == null) { return NotFound(); }
                else
                {
                    // in this moment we assume that email, permissionid 
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
        [HttpDelete("{email_s}", Name = "DeleteUser")]
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
    }
}
