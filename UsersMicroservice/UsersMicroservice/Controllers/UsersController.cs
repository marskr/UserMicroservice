using Microsoft.AspNetCore.Mvc;
using UsersMicroservice.Data;
using Newtonsoft.Json;

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

        // GET api/users
        [HttpGet]
        public string Get()
        {
            var json = JsonConvert.SerializeObject(_context.Users);

            return json;
        }

        // GET api/users/{id}
        [HttpGet("{id}")]
        public string Get(int id)
        {

            var settings = _context.Users.Find(id);
            var json = JsonConvert.SerializeObject(settings);
            if (settings == null)
            {
                return "{}";
            }

            return json;
        }

        // POST api/users
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/users/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/users/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _context.Users.Remove(_context.Users.Find(id));
            _context.SaveChanges();
        }
    }
}
