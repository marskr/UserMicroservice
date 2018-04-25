using System;
using System.Linq;
using UsersMicroservice.Data;
using UsersMicroservice.JWT;
using UsersMicroservice.Logs;
using UsersMicroservice.Models;

namespace UsersMicroservice.Queries
{
    public class UserAuthQueriesFactory : AbstractQueriesFactory<Users, AppDbContext>
    {
        JWTManager _jwt = new JWTManager();
        private readonly string _logInfo = "[AuthQueries]";

        public override string APICreateToken(string email_s, string password_s)
        {
            // create token before hashing password!
            ErrInfLogger.LockInstance.InfoLog("APICreateToken launched." + _logInfo);
            return _jwt.ReturnJWT(email_s, password_s);
        }

        public override Users APIGet(string token_s, AppDbContext context)
        {
            ErrInfLogger.LockInstance.InfoLog("APIGet launched." + _logInfo);
            return context.Users.FirstOrDefault(t => t.AuthToken == token_s);
        }

        public override void APIDelete(Users deletedUser, AppDbContext context)
        {
            throw new System.NotImplementedException();
        }

        public override void APIPut(Users updatedUser, Users newUser, AppDbContext context)
        {
            ErrInfLogger.LockInstance.InfoLog("APIPut launched." + _logInfo);
            updatedUser.AuthTokenExpiration = DateTime.Now.AddHours(1);
            context.SaveChanges();
        }

        public override void APIPost(Users newUser, AppDbContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}
