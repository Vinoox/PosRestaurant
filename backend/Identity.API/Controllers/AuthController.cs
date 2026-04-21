using System.Threading.Tasks;
using Identity.Application.Auth.Commands;
using Identity.Application.Auth.Queries;
using Identity.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Identity.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Rejestracja nowego użytkownika")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
        {
            await _authService.RegisterAsync(dto);
            return Ok();
        }

        [HttpPost("login")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Logowanie użytkownika i pobranie tokenu JWT")]
        public async Task<IActionResult> Login([FromBody] AuthenticateDto dto)
        {
            var result = await _authService.AuthenticateAsync(dto);
            return Ok(result);
        }

        //[HttpPost("login-pin")]
        //[AllowAnonymous]
        //[SwaggerOperation(Summary = "Logowanie za pomocą PINu")]
        //public async Task<IActionResult> LoginByPin([FromBody] LoginByPinDto dto)
        //{
        //    var token = await _authService.LoginByPinAsync(dto);
        //    return Ok(new { Token = token });
        //}
    }
}