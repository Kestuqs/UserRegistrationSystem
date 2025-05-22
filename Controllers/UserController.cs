using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.InMemory.Design.Internal;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Security.Claims;

namespace UserRegistrationSystem.API.Controllers
{
    [ApiController]
    [Route("api/user")]
    //[Authorize] // Reikalauja prisijungimo su JWT
    public class UserController : ControllerBase
    {
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var username = User.FindFirstValue(ClaimTypes.Name);
            var role = User.FindFirstValue(ClaimTypes.Role);

            return Ok(new
            {
                Message = "You are authorized!",
                Username = username,
                Role = role,
                UserId = userId
            });
        }

        [HttpGet("admin")]
        
        public IActionResult AdminOnlyEndpoint()
        {
            return Ok(new { Message = "You are an Admin!" });
        }
    }
}
