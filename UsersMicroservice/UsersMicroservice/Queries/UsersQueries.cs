using System.Linq;
using UsersMicroservice.Data;
using UsersMicroservice.Encryption;
using UsersMicroservice.JWT;
using UsersMicroservice.Models;

namespace UsersMicroservice.Queries
{
    public class UserCRUDQueriesFactory : AbstractQueriesFactory<Users, AppDbContext>
    {
        EncryptionManager _krypton = new EncryptionManager();
        JWTManager _jwt = new JWTManager();

        public override Users APIGet(string email_s, AppDbContext context)
        {
            return context.Users.FirstOrDefault(t => t.Email == email_s);
        }

        public override void APIPost(Users newUser, AppDbContext context)
        {
            // create token before hashing password
            newUser.AuthToken = _jwt.ReturnJWT(newUser.Email, newUser.HashPassword);
            
            // encrypt password 
            newUser.HashPassword = _krypton.EncryptStringAES(newUser.HashPassword, newUser.Salt); 
            context.Users.Add(newUser);
            context.SaveChanges();
        }

        public override void APIPut(Users updatedUser, Users newUser, AppDbContext context)
        {
            //updatedUser.Email = newUser.Email;
            updatedUser.Name = newUser.Name;
            updatedUser.Surname = newUser.Surname;
            updatedUser.PhoneNumber = newUser.PhoneNumber;
            updatedUser.AuthToken = _jwt.ReturnJWT(updatedUser.Email, updatedUser.HashPassword);
            updatedUser.HashPassword = _krypton.EncryptStringAES(newUser.HashPassword, updatedUser.Salt); 
            context.SaveChanges();
        }

        public override void APIDelete(Users deletedUser, AppDbContext context)
        {
            context.Users.Remove(deletedUser);
            context.SaveChanges();
        }

        public override string APICreateToken(string email_s, string password_s)
        {
            throw new System.NotImplementedException();
        }
    }
}
