using System.Threading.Tasks;
using Identity.Application.Interfaces;
using Identity.Application.Users.Dtos.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;
using Identity.Application.Users.Commands.ChangePassword;
using Identity.Application.Users.Commands.ChangePin;
using Identity.Application.Users.Commands.UpdateUserProfile;

namespace Identity.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("me")]
        [SwaggerOperation(Summary = "Pobranie danych aktualnie zalogowanego użytkownika")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userService.GetByIdAsync(userId!);
            return Ok(user);
        }

        [HttpPut("me/profile")]
        [SwaggerOperation(Summary = "Aktualizacja profilu użytkownika")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateUserProfileCommand dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _userService.UpdateProfileAsync(userId!, dto);
            return NoContent();
        }

        [HttpPut("me/password")]
        [SwaggerOperation(Summary = "Zmiana hasła użytkownika")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _userService.ChangePasswordAsync(userId!, dto);
            return NoContent();
        }

        [HttpPut("me/pin")]
        [SwaggerOperation(Summary = "Zmiana PINu użytkownika")]
        public async Task<IActionResult> ChangePin([FromBody] ChangePinCommand dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _userService.ChangePinAsync(userId!, dto);
            return NoContent();
        }
    }
}