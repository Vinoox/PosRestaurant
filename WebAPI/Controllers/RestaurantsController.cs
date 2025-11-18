using System.Security.Claims;
using Application.Features.Restaurants.Dtos.Commands;
using Application.Features.StaffManagement.Dtos.Commands;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace WebAPI.Controllers
{
    [Route("api/restaurants")]
    [ApiController]
    [Authorize]
    public class RestaurantsController : ControllerBase
    {
        private readonly IRestaurantService _restaurantService;

        public RestaurantsController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Get restaurant by ID")]
        public async Task<IActionResult> GetById(int id)
        {
            var restaurantDto = await _restaurantService.GetByIdAsync(id);
            return Ok(restaurantDto);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create a new restaurant")]
        [ProducesResponseType(typeof(object), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] CreateRestaurantDto dto)
        {
            var creatorUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var restaurantId = await _restaurantService.CreateAsync(dto, creatorUserId);

            return CreatedAtAction(nameof(GetById), new { id = restaurantId }, new { Id = restaurantId });
        }
    }
}
