using Application.Features.Users.Dtos;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Features.Users
{
    public interface IUserService
    {
        Task<string> LoginAsync(LoginDto dto);
        Task<string> LoginByPinAsync(LoginByPinDto dto);
        Task<IdentityResult> RegisterAsync(RegisterUserDto dto);
        Task<IdentityResult> ChangePasswordAsync(string userId, ChangePasswordDto dto);
        Task UpdateProfileAsync(string userId, UpdateUserProfileDto dto);
        Task ChangePinAsync(string userId, ChangePinDto dto);
        Task DeleteAsync(string userId);
        Task<UserDto?> GetUserByIdAsync(string userId);
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
    }
}