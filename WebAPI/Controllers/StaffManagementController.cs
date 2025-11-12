using Application.Features.StaffManagement.Dtos.Commands;
using Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace WebAPI.Controllers
{
    [Route("api/restaurants/{restaurantId}/staff")]
    [ApiController]
    [Authorize]
    public class StaffManagementController : ControllerBase
    {
        private readonly IStaffManagementService _staffManagementService;
        private readonly IValidator<AddStaffMemberDto> _addStaffMemberDtoValidator;
        private readonly IValidator<RemoveStaffMemberDto> _removeStaffMemberDtoValidator;
        private readonly IValidator<ChangeStaffMemberRoleDto> _changeStaffMemberRoleDtoValidator;

        public StaffManagementController(
            IStaffManagementService staffManagementService,
            IValidator<AddStaffMemberDto> addStaffMemberValidator,
            IValidator<RemoveStaffMemberDto> removeStaffMemberDtoValidator,
            IValidator<ChangeStaffMemberRoleDto> changeStaffMemberRoleDto)
        {
            _staffManagementService = staffManagementService;
            _addStaffMemberDtoValidator = addStaffMemberValidator;
            _removeStaffMemberDtoValidator = removeStaffMemberDtoValidator;
            _changeStaffMemberRoleDtoValidator = changeStaffMemberRoleDto;
        }

        [HttpPost]
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

        [HttpDelete("{dto.Email}")]
        [Authorize(Policy = "IsRestaurantAdmin")]
        [SwaggerOperation(Summary = "Remove a staff member from a restaurant")]
        public async Task<IActionResult> RemoveStaffMember([FromRoute] int restaurantId, [FromBody] RemoveStaffMemberDto dto)
        {
            var validationResult = await _removeStaffMemberDtoValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var adminRestaurantIdClaim = User.FindFirst("RestaurantId");
            if (adminRestaurantIdClaim == null || adminRestaurantIdClaim.Value != restaurantId.ToString())
            {
                return Forbid("Nie masz uprawnień do zarządzania tą restauracją");
            }

            await _staffManagementService.RemoveStaffMemberAsync(restaurantId, dto);
            return NoContent();
        }

        [HttpPost("{dto.Email}/role")]
        [Authorize(Policy = "IsRestaurantAdmin")]
        [SwaggerOperation(Summary = "Change a role of staff member")]
        public async Task<IActionResult> ChangeMemberRole([FromRoute] int restaurantId, [FromBody] ChangeStaffMemberRoleDto dto)
        {
            var validationResult = await _changeStaffMemberRoleDtoValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var adminRestaurantIdClaim = User.FindFirst("RestaurantId");
            if (adminRestaurantIdClaim == null || adminRestaurantIdClaim.Value != restaurantId.ToString())
            {
                return Forbid("Nie masz uprawnień do zarządzania tą restauracją");
            }

            await _staffManagementService.ChangeStaffMemberRoleAsync(restaurantId, dto);
            return NoContent();
        }
    }
}
