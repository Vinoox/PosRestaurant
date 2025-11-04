using System.Threading.Tasks;
using Application.Features.Products;
using Application.Features.Products.Dtos;
using Application.Features.Users;
using Application.Features.Users.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [SwaggerOperation(Summary = "Get all users")]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        [SwaggerOperation(Summary = "Get user by id")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [SwaggerOperation(Summary = "Register new user")]
        [HttpPost]
        public async Task<IActionResult> RegisterUser(RegisterUserDto newUser)
        {
            var user = await _userService.RegisterAsync(newUser);
            return Created($"api/users/{user.Id}", user);
        }


        [SwaggerOperation(Summary = "Update existing user")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UpdateUserDto updatedUser)
        {
            await _userService.UpdateAsync(id, updatedUser);
            return NoContent();
        }

        [SwaggerOperation(Summary = "Delete user")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _userService.DeleteAsync(id);
            return NoContent();
        }
    }
}
