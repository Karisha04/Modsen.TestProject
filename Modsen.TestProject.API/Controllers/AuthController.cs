using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modsen.TestProject.Domain.Interfaces;
using Modsen.TestProject.Domain.Models;

namespace Modsen.TestProject.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;

        public AuthController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel model)
        {
            if (model.Username == "admin" && model.Password == "password") 
            {
                var token = _tokenService.GenerateToken(model.Username, "Admin");
                return Ok(new { Token = token });
            }

            return Unauthorized("Invalid credentials");
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpGet("secure")]
        public IActionResult SecureEndpoint()
        {
            return Ok("You are authorized!");
        }
    }

}
