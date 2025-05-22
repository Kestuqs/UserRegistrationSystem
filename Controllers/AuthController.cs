using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UserRegistrationSystem.Application.DTO;
using UserRegistrationSystem.Application.Interfaces;
using UserRegistrationSystem.Domain.Models; // Reikalinga dėl User tipo

namespace UserRegistrationSystem.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ITokenService _tokenService;

        public AuthController(IAuthService authService, ITokenService tokenService)
        {
            _authService = authService;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] RegisterUserDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.RegisterAsync(dto);
            if (result)
            {
                return Ok(new { Message = "User registered successfully" });
            }
            else
            {
                return BadRequest(new { Message = "User registration failed" });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] LoginUserDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User? user = await _authService.LoginAsync(dto);
            if (user == null)
            {
                return Unauthorized(new { Message = "Invalid credentials" });
            }

            string token = _tokenService.CreateToken(user);

            return Ok(new
            {
                Token = token,
                Username = user.Username,
                Role = user.Role
            });
        }
    }
}


