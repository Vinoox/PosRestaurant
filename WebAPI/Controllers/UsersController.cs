using Application.Features.Users;
using Application.Features.Users.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/users")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [SwaggerOperation(Summary = "Get all users (Admin only)")]
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [SwaggerOperation(Summary = "Get user by id")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [SwaggerOperation(Summary = "Update user profile")]
        [HttpPut("{id}/profile")]
        public async Task<IActionResult> UpdateProfile(string id, UpdateUserProfileDto dto)
        {
            // UWAGA: Należałoby sprawdzić, czy zalogowany użytkownik ma prawo aktualizować ten profil
            // (np. czy to jego własny profil, lub czy jest adminem). To kolejny krok w rozbudowie.
            await _userService.UpdateProfileAsync(id, dto);
            return NoContent(); // 204 No Content to standardowa odpowiedź dla udanej aktualizacji.
        }

        [SwaggerOperation(Summary = "Change user password")]
        [HttpPost("{id}/change-password")]
        public async Task<IActionResult> ChangePassword(string id, ChangePasswordDto dto)
        {
            var result = await _userService.ChangePasswordAsync(id, dto);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            return NoContent();
        }

        [SwaggerOperation(Summary = "Delete user (Admin only)")]
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")] // Tylko Admin może usuwać użytkowników
        public async Task<IActionResult> DeleteUser(string id) // ZMIANA: id jest teraz stringiem
        {
            await _userService.DeleteAsync(id);
            return NoContent();
        }
    }
}