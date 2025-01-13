using Microsoft.EntityFrameworkCore;
using Modsen.TestProject.DAL;
using Modsen.TestProject.Domain.Interfaces;
using Modsen.TestProject.Domain.Models;
using System.Collections.Generic;

namespace Modsen.TestProject.Application.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ProjectDbContext _context;


        private static List<User> _users = new List<User>
        {
            new User
            {
                Username = "admin",
                HashedPassword = "somesaltedhashedpassword",
                Role = "Admin"
            }
        };

        public User GetUserByUsername(string username)
        {
            return _users.FirstOrDefault(u => u.Username == username);
        }

        public void SaveRefreshToken(string username, string refreshToken)
        {
            var user = _users.FirstOrDefault(u => u.Username == username);

            if (user != null)
            {
                user.RefreshToken = refreshToken;

                _context.SaveChanges(); 
            }
            else
            {
                throw new Exception("Пользователь не найден.");
            }
        }

    }
}
