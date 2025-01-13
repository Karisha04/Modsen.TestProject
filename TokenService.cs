using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Modsen.TestProject.Domain.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Modsen.TestProject.Application.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Генерация Access Token
        public string GenerateAccessToken(string username, string role)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),  // Длительность Access Token
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // Генерация Refresh Token
        public string GenerateRefreshToken()
        {
            // Генерация случайного токена
            var refreshToken = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            return refreshToken;
        }

        // Логика для проверки и обновления токенов (при получении Refresh Token)
        public (string accessToken, string refreshToken) RefreshTokens(string refreshToken)
        {
            // Здесь нужно добавить логику для проверки refresh token.
            // Например, можно хранить refresh tokens в базе данных и проверять его действительность.

            // Генерация новых токенов
            var newAccessToken = GenerateAccessToken("username", "role");  // Параметры username и role должны быть извлечены из хранения/креденциалов
            var newRefreshToken = GenerateRefreshToken();

            return (newAccessToken, newRefreshToken);
        }
    }
}
