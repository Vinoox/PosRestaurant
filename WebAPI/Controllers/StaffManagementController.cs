using Application.Features.StaffManagement.Dtos.Commands;
using Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace WebAPI.Controllers
{
    public class StaffManagementController : ControllerBase
    {
        private readonly IStaffManagementService _staffManagementService;
        private readonly IValidator<AddStaffMemberDto> _addStaffMemberDtoValidator;

        public StaffManagementController(
            IStaffManagementService staffManagementService,
            IValidator<AddStaffMemberDto> addStaffMemberValidator)
        {
            _staffManagementService = staffManagementService;
            _addStaffMemberDtoValidator = addStaffMemberValidator;
        }

        [HttpPost("api/restaurants/{restaurantId}/staff")]
        [Authorize(Policy = "IsRestaurantAdmin")]
        [SwaggerOperation(Summary = "Add a staff member to a restaurant")]
        public async Task<IActionResult> AddStaffMember([FromRoute] int restaurantId, [FromBody]AddStaffMemberDto dto)
        {
            var validationResult = await _addStaffMemberDtoValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var adminRestaurantIdClaim = User.FindFirst("RestaurantId");
            if (adminRestaurantIdClaim == null || adminRestaurantIdClaim.Value != restaurantId.ToString())
            {
                return Forbid("Nie masz uprawnień do zarządzania tą restauracją");
            }

            await _staffManagementService.AddStaffMemberAsync(restaurantId, dto);
            return NoContent();
        }

        [HttpPost("api/restaurants/{restaurantId}/staff")]
        [Authorize(Policy = "IsRestaurantAdmin")]
        [SwaggerOperation(Summary = "Add a staff member to a restaurant")]
        public async Task<IActionResult> AddStaffMember([FromRoute] int restaurantId, [FromBody] AddStaffMemberDto dto)
        {
            var validationResult = await _addStaffMemberDtoValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var adminRestaurantIdClaim = User.FindFirst("RestaurantId");
            if (adminRestaurantIdClaim == null || adminRestaurantIdClaim.Value != restaurantId.ToString())
            {
                return Forbid("Nie masz uprawnień do zarządzania tą restauracją");
            }

            await _staffManagementService.AddStaffMemberAsync(restaurantId, dto);
            return NoContent();
        } 
    }
}
