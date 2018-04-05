using System.Linq;
using UsersMicroservice.Data;
using UsersMicroservice.Encryption;
using UsersMicroservice.Models;

namespace UsersMicroservice.Queries
{
    public class UserQueriesFactory : AbstractQueriesFactory<Users, AppDbContext>
    {
        EncryptionManager _krypton = new EncryptionManager();

        public override Users APIGet(string email_s, AppDbContext context)
        {
            Users getQuery = context.Users.FirstOrDefault(t => t.Email == email_s);
            if (getQuery == null) { return getQuery; }
            //getQuery.HashPassword = _krypton.DecryptStringAES(getQuery.HashPassword, getQuery.Salt);
            return getQuery;
        }

        public override void APIPost(Users newUser, AppDbContext context)
        {
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
            updatedUser.HashPassword = _krypton.EncryptStringAES(newUser.HashPassword, updatedUser.Salt); 
            context.SaveChanges();
        }

        public override void APIDelete(Users deletedUser, AppDbContext context)
        {
            context.Users.Remove(deletedUser);
            context.SaveChanges();
        }
    }
}
