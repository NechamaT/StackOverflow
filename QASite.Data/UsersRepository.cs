using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QASite.Data
{
    public class UsersRepository
    {
        private readonly string _connectionString;

        public UsersRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public void AddUser(User user, string password)
        {
            using var ctx = new QuestionTagsContext(_connectionString);
            user.HashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            ctx.Users.Add(user);
            ctx.SaveChanges();
        }

        public User GetUserByEmail(string email)
        {
            using var ctx = new QuestionTagsContext(_connectionString);
            return ctx.Users.FirstOrDefault(u => u.Email == email);
        }

        public User LogIn(string password, string email)
        {
            var user = GetUserByEmail(email);
            if (user == null)
            {
                return null;
            }

            bool isValid = BCrypt.Net.BCrypt.Verify(password, user.HashedPassword);
            if (isValid)
            {
                return user;
            }

            return null;
        }

        public bool IsEmailAvailable(string email)
        {
            using var ctx = new QuestionTagsContext(_connectionString);
            return !ctx.Users.Any(u => u.Email == email);
        }

        public bool CanLike(int userId)
        {
            using var ctx = new QuestionTagsContext(_connectionString);
            var like = ctx.Likes.FirstOrDefault(l => l.UserId == userId);
            return like == null ? true : false;
        }

      

    }
}

