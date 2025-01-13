using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Modsen.TestProject.Domain.Interfaces;
using Modsen.TestProject.Domain.Models;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ITokenService _tokenService;
    private readonly IUserRepository _userRepository;

    public AuthController(ITokenService tokenService, IUserRepository userRepository)
    {
        _tokenService = tokenService;
        _userRepository = userRepository;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginModel model)
    {
        var user = _userRepository.GetUserByUsername(model.Username);

        if (user == null)
            return Unauthorized("Invalid credentials");

        if (!VerifyPassword(model.Password, user.HashedPassword))
            return Unauthorized("Invalid credentials");

        var accessToken = _tokenService.GenerateAccessToken(user.Username, user.Role);
        var refreshToken = _tokenService.GenerateRefreshToken();

        _userRepository.SaveRefreshToken(user.Username, refreshToken);

        return Ok(new { AccessToken = accessToken, RefreshToken = refreshToken });
    }

    [HttpPost("refresh")]
    public IActionResult Refresh([FromBody] string refreshToken)
    {
        var tokens = _tokenService.RefreshTokens(refreshToken);

        return Ok(new { AccessToken = tokens.accessToken, RefreshToken = tokens.refreshToken });
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpGet("secure")]
    public IActionResult SecureEndpoint()
    {
        return Ok("You are authorized!");
    }

    private bool VerifyPassword(string password, string hashedPassword)
    {
        byte[] salt = Convert.FromBase64String(hashedPassword.Substring(0, 24));
        string storedHash = hashedPassword.Substring(24);

        string hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 10000,
            numBytesRequested: 32));

        return hash == storedHash;
    }
}
