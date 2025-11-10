using Application.Features.Users.Dtos.Commands;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [SwaggerOperation(Summary = "Register new user")]
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserDto dto)
        {
            var result = await _userService.RegisterAsync(dto);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(new { Message = "Rejestracja pomyślna." });
        }

        [HttpPost("authenticate")]
        [SwaggerOperation(Summary = "Step 1: Authenticates a user and returns available restaurants")]
        public async Task<IActionResult> Authenticate(AuthenticateDto dto)
        {
            // ZMIANA: Poprawiona składnia i nazwy zmiennych
            var result = await _userService.AuthenticateAsync(dto);

            if (result == null)
            {
                return Unauthorized(new { Message = "Nieprawidłowy email lub hasło." });
            }

            return Ok(result);
        }

        [HttpPost("select-restaurant")]
        [Authorize]
        [SwaggerOperation(Summary = "Step 2: Selects a restaurant context and returns the final JWT")]
        public async Task<IActionResult> SelectRestaurant(SelectRestaurantDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Nieprawidłowy token uwierzytelniający.");
            }

            var token = await _userService.GenerateContextualTokenAsync(userId, dto.RestaurantId);
            return Ok(new { Token = token });
        }
    }
}