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
            getQuery.Password = _krypton.DecryptStringAES(getQuery.Password, getQuery.Salt);
            return getQuery;
        }

        public override void APIPost(Users newUser, AppDbContext context)
        {
            newUser.Id = context.Users.Max(t => t.Id) + 10;
            newUser.Password = _krypton.EncryptStringAES(newUser.Password, newUser.Salt); 
            context.Users.Add(newUser);
            context.SaveChanges();
        }

        public override void APIPut(Users updatedUser, Users newUser, AppDbContext context)
        {
            updatedUser.Name = newUser.Name;
            updatedUser.Surname = newUser.Surname;
            updatedUser.Password = _krypton.EncryptStringAES(newUser.Password, updatedUser.Salt); 
            context.SaveChanges();
        }

        public override void APIDelete(Users deletedUser, AppDbContext context)
        {
            context.Users.Remove(deletedUser);
            context.SaveChanges();
        }
    }
}
