using WebApplication2.Models;

namespace WebApplication2.Service
{
    public interface UserService
    {
        public bool Create(User user);
        public bool Login(string useremail, string userpassword);
        public bool Update(User user);
        public User findByEmail(string useremail);
    }
}
