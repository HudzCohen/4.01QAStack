using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4._01QAStack.Data
{
    public class UserRepo
    {
        private readonly string _connectionString;

        public UserRepo(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddUser(User user, string password)
        {
            string hash = BCrypt.Net.BCrypt.HashPassword(password);
            user.PasswordHash = hash;
            using var ctx = new QAStackDataContext(_connectionString);
            ctx.Users.Add(user);
            ctx.SaveChanges();
        }

        public User Login(string email, string password)
        {
            var user = GetByEmail(email);
            if (user == null)
            {
                return null;
            }

            bool isValidPassword = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
            if (isValidPassword)
            {
                return user; 
            }

            return null;
        }

        public User GetByEmail(string email)
        {
            using var ctx = new QAStackDataContext(_connectionString);
            return ctx.Users.FirstOrDefault(u => u.Email == email);
        }

        public bool EmailAvailable(string email)
        {
            using var ctx = new QAStackDataContext(_connectionString);
            return !ctx.Users.Any(u => u.Email == email);
        }
    }

}
