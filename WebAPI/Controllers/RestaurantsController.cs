using System.Security.Claims;
using Application.Features.Restaurants.Dtos.Commands;
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

        [HttpPost]
        [SwaggerOperation(Summary = "Create a new restaurant")]
        public async Task<IActionResult> Create(CreateRestaurantDto dto)
        {
            var creatorUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if(string.IsNullOrEmpty(creatorUserId))
            {
                return Unauthorized();
            }
            try
            {
                var restaurantId = await _restaurantService.CreateAsync(dto, creatorUserId);
                return CreatedAtAction(nameof(GetById), new { id = restaurantId }, new { Id = restaurantId });
            }
            catch(InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while creating the restaurant.", Details = ex.Message });
            }
        }

        [HttpPost("{restaurantId}/staff")]
        [Authorize(Policy = "IsRestaurantAdmin")]
        [SwaggerOperation(Summary = "Add a staff member to a restaurant")]
        public async Task<IActionResult> AddStaffMember([FromRoute] int restaurantId, [FromBody] AddStaffMemberDto dto)
        {
            var adminRestaurantIdClaim = User.FindFirst("restaurantId");
            if (adminRestaurantIdClaim == null || adminRestaurantIdClaim.Value != restaurantId.ToString())
            {
                return Forbid("Nie masz uprawnień do zarządzania tą restauracją");
            }
            try
            {
                await _restaurantService.AddStaffMemberAsync(restaurantId, dto);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while adding the staff member.", Details = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Get restaurant by ID")]
        public async Task<IActionResult> GetById(int id)
        {
            var restaurant = await _restaurantService.GetByIdAsync(id);
            if (restaurant == null)
            {
                return NotFound();
            }
            return Ok(restaurant);
        }
    }
}
