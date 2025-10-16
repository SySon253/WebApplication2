using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Generators;
using WebApplication2.Data;
using WebApplication2.Models;

namespace WebApplication2.Service
{
    public class UserServiceImpl : UserService
    {
        private WebApplication2Context db;
        public UserServiceImpl(WebApplication2Context _db)
        {
            db = _db;
        }

        //public bool Create(Account account)
        //{
        //    try
        //    {
        //        db.Accounts.Add(account);
        //        return db.SaveChanges() > 0;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}
        public bool Create(User user)
        {
            try
            {
                if (db.User.Any(a => a.UserEmail == user.UserEmail))
                {
                    Console.WriteLine("Email already exists.");
                    return false;
                }

                db.User.Add(user);
                var result = db.SaveChanges() > 0;
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error creating user: " + ex.Message);
                return false;
            }
        }

        public User findByEmail(string email)
        {
            return db.User.AsNoTracking().SingleOrDefault(a => a.UserEmail == email);
        }

        public bool Login(string useremail, string userpassword)
        {
            var user = db.User.SingleOrDefault(a => a.UserEmail == useremail);
            if (user != null)
            {
                return BCrypt.Net.BCrypt.Verify(userpassword, user.UserPassword);
            }
            return false;
        }

        public bool Update(User user)
        {
            try
            {
                db.Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                return db.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }
    }
}
