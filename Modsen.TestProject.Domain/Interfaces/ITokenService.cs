namespace Modsen.TestProject.Domain.Interfaces
{
    public interface ITokenService
    {
        string GenerateAccessToken(string username, string role);
        string GenerateRefreshToken();
        (string accessToken, string refreshToken) RefreshTokens(string refreshToken);
    }
}
