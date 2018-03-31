using Microsoft.AspNetCore.Mvc;
using UsersMicroservice.Data;
using System.Linq;
using UsersMicroservice.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace UsersMicroservice.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        //// GET api/users
        //[HttpGet]
        //public IActionResult Get()
        //{
        //    DbSet<Users> allUsers =_context.Users;
        //    if (allUsers == null) { return NotFound(); }

        //    return new ObjectResult(allUsers);
        //}

        // GET api/users/{email_s}
        [HttpGet("{email_s}", Name = "GetSingleUser")]
        public IActionResult Get(string email_s)
        {
            Users specifiedUser = _context.Users.FirstOrDefault(t => t.Email == email_s);
            if (specifiedUser == null) { return NotFound(); }

            return new ObjectResult(specifiedUser);
        }

        // GET api/users/top/{number_i}
        [HttpGet("top/{number_i}")]
        public IActionResult GetTop(int number_i)
        {
            IOrderedQueryable topUsers = _context.Users.Take(number_i).OrderBy(t => t.Email);
            if (topUsers == null) { return NotFound(); }

            return new ObjectResult(topUsers);
        }

        // POST api/users/
        [HttpPost]
        public IActionResult Post([FromBody]Users newUser)
        {
            try
            {
                if (newUser == null) { return BadRequest(); }
                _context.Users.Add(newUser);
                _context.SaveChanges();
            }
            catch
            {
                return BadRequest();
            }
            
            return CreatedAtRoute("GetSingleUser", new { email_s = newUser.Email }, newUser);
        }

        // PUT api/users/5
        [HttpPut("{email_s}")]
        public IActionResult Put(string email_s, [FromBody]Users updatedUser)
        {
            try
            {
                var specifiedUser = _context.Users.FirstOrDefault(t => t.Email == email_s);
                if (specifiedUser == null) { return NotFound(); }
                else
                {
                    // in this moment we assume that id, email, salt, authtoken, authtokenexpiration & permissionid 
                    // is UNALTERABLE!!

                    //specifiedUser.Id = updatedUser.Id;
                    //specifiedUser.Email = updatedUser.Email;
                    specifiedUser.Name = updatedUser.Name;
                    specifiedUser.Surname = updatedUser.Surname;
                    specifiedUser.Password = updatedUser.Password;
                    //specifiedUser.Salt = updatedUser.Salt;
                    //specifiedUser.AuthToken = updatedUser.AuthToken;
                    //specifiedUser.AuthTokenExpiration = updatedUser.AuthTokenExpiration;
                    //specifiedUser.PermissionId = updatedUser.PermissionId;
                    _context.SaveChanges();
                }
            }
            catch
            {
                return BadRequest();
            }

            return CreatedAtRoute("GetSingleUser", new { email_s = updatedUser.Email }, updatedUser);
        }

        // DELETE api/users/{email}
        [HttpDelete("{email_s}")]
        public IActionResult Delete(string email_s)
        {
            try
            {
                EntityEntry settings = _context.Users.Remove(_context.Users.FirstOrDefault(t => t.Email == email_s));
                if (settings == null) { return NotFound(); }
                _context.SaveChanges();
            }
            catch
            {
                return BadRequest();
            }
            
            return new NoContentResult();
        }
    }
}
