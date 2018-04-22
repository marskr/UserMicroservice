using System;
using System.Linq;
using UsersMicroservice.Data;
using UsersMicroservice.JWT;
using UsersMicroservice.Models;

namespace UsersMicroservice.Queries
{
    public class UserAuthQueriesFactory : AbstractQueriesFactory<Users, AppDbContext>
    {
        JWTManager _jwt = new JWTManager();

        public override string APICreateToken(string email_s, string password_s)
        {
            // create token before hashing password!
            return _jwt.ReturnJWT(email_s, password_s);
        }

        public override Users APIGet(string token_s, AppDbContext context)
        {
            return context.Users.FirstOrDefault(t => t.AuthToken == token_s);
        }

        public override void APIDelete(Users deletedUser, AppDbContext context)
        {
            throw new System.NotImplementedException();
        }

        public override void APIPut(Users updatedUser, Users newUser, AppDbContext context)
        {
            updatedUser.AuthTokenExpiration = DateTime.Now.AddHours(1);
            context.SaveChanges();
        }

        public override void APIPost(Users newUser, AppDbContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}
