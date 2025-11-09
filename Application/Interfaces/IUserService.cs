using Application.Features.Users.Dtos.Commands;
using Application.Features.Users.Dtos.Queries;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUserService
    {
        //Task<string> LoginByPinAsync(LoginByPinDto dto);
        Task<AuthenticationResultDto?> AuthenticateAsync(AuthenticateDto dto);
        Task<string> GenerateContextualTokenAsync(string userId, int restaurantId);
        Task<IdentityResult> RegisterAsync(RegisterUserDto dto);
        Task<IdentityResult> ChangePasswordAsync(string userId, ChangePasswordDto dto);
        Task UpdateProfileAsync(string userId, UpdateUserProfileDto dto);
        Task ChangePinAsync(string userId, ChangePinDto dto);
        Task DeleteAsync(string userId);
        Task<UserDto?> GetUserByIdAsync(string userId);
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
    }
}