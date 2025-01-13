using Modsen.TestProject.Domain.Models;

namespace Modsen.TestProject.Domain.Interfaces
{
    public interface IUserRepository
    {
        User GetUserByUsername(string username);
        void SaveRefreshToken(string username, string refreshToken);
    }
}
