using System;
using System.Linq;
using UsersMicroservice.Data;
using UsersMicroservice.Encryption;
using UsersMicroservice.JWT;
using UsersMicroservice.Logs;
using UsersMicroservice.Models;

namespace UsersMicroservice.Queries
{
    public class UserAuthQueriesFactory : AbstractQueriesFactory<Users, AppDbContext>
    {
        EncryptionManager _krypton = new EncryptionManager();
        JWTManager _jwt = new JWTManager();
        private readonly string _logInfo = "[AuthQueries]";

        public override string APICreateToken(string email_s, string password_s, AppDbContext context)
        {
            ErrInfLogger.LockInstance.InfoLog("APICreateToken launched." + _logInfo);
            Users specifiedUser = context.Users.FirstOrDefault(t => t.Email == email_s);
            if (specifiedUser == null) { return null; }
            if (Equals(_krypton.DecryptStringAES(specifiedUser.HashPassword, specifiedUser.Salt), password_s))
            {
                DateTime tokenExpiration = DateTime.Now.AddHours(12);

                int userId_i = (specifiedUser.Id.HasValue) ? specifiedUser.Id.Value : -1;

                if (userId_i == -1) { return null; }

                int permissionId_i = (specifiedUser.PermissionId.HasValue) ? specifiedUser.PermissionId.Value : 0;

                string token_s = _jwt.ReturnJWT(tokenExpiration, permissionId_i, userId_i);

                specifiedUser.AuthTokenExpiration = tokenExpiration;
                specifiedUser.AuthToken = token_s;
                context.SaveChanges();
                return token_s;
            }
            else { return null; }
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
            throw new System.NotImplementedException();
        }

        public override void APIPost(Users newUser, AppDbContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}
