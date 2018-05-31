using Microsoft.EntityFrameworkCore;
using System;
using System.Data.SqlClient;
using System.Linq;
using UsersMicroservice.Data;
using UsersMicroservice.Encryption;
using UsersMicroservice.JWT;
using UsersMicroservice.Logs;
using UsersMicroservice.Models;

namespace UsersMicroservice.Queries
{
    public class UserCRUDQueriesFactory : AbstractQueriesFactory<Users, AppDbContext>
    {
        EncryptionManager _krypton = new EncryptionManager();
        JWTManager _jwt = new JWTManager();
        private readonly string _logInfo = "[Queries]";

        public override Users APIGet(string email_s, AppDbContext context)
        {
            ErrInfLogger.LockInstance.InfoLog("APIGet launched." + _logInfo);

            return context.Users.FirstOrDefault(t => t.Email == email_s); ;
        }

        public override void APIPost(Users newUser, AppDbContext context)
        {
            ErrInfLogger.LockInstance.InfoLog("APIPost launched." + _logInfo);
            
            SqlParameter result = new SqlParameter("@result", System.Data.SqlDbType.Int)
            {
                Direction = System.Data.ParameterDirection.Output
            };

            context.Database.ExecuteSqlCommand("SELECT @result = (NEXT VALUE FOR IntSeq)", result);
            
            //(int)result.Value
            ErrInfLogger.LockInstance.InfoLog(result.Value.ToString());

            newUser.Id = (int)result.Value;
            newUser.PermissionId = 0;
            newUser.AuthToken = _jwt.ReturnJWT(System.DateTime.Now, 0, (int)result.Value);
            newUser.AuthTokenExpiration = System.DateTime.Now;
            newUser.Salt = SaltGenerator.GenerateSalt();
            newUser.UserAccountStatus = "normal";

            // encrypt password 
            newUser.HashPassword = _krypton.EncryptStringAES(newUser.HashPassword, newUser.Salt); 
            context.Users.Add(newUser);

            context.SaveChanges();
        }

        public override void APIPut(Users updatedUser, Users newUser, AppDbContext context)
        {
            ErrInfLogger.LockInstance.InfoLog("APIPut launched." + _logInfo);

            updatedUser.Name = newUser.Name;
            updatedUser.Surname = newUser.Surname;
            updatedUser.PhoneNumber = newUser.PhoneNumber;
            updatedUser.HashPassword = _krypton.EncryptStringAES(newUser.HashPassword, updatedUser.Salt); 

            context.SaveChanges();
        }

        public override void APIDelete(Users deletedUser, AppDbContext context)
        {
            ErrInfLogger.LockInstance.InfoLog("APIDelete launched." + _logInfo);

            context.Users.Remove(deletedUser);

            context.SaveChanges();
        }

        public override string APICreateToken(string email_s, string password_s, AppDbContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}
