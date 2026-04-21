using Identity.Application.Users.Dtos.Commands;
using Identity.Application.Users.Dtos.Queries;
using Identity.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Identity.Application.Interfaces
{
    public interface IUserService
    {
        Task<IdentityResult> ChangePasswordAsync(string userId, ChangePasswordDto dto);
        Task UpdateProfileAsync(string userId, UpdateUserProfileDto dto);
        Task ChangePinAsync(string userId, ChangePinDto dto);
        Task DeleteAsync(string userId);
        Task<UserDto?> GetByIdAsync(string userId);
        IEnumerable<UserDto> GetAllUsers();
        Task<User> FindByEmailOrThrowAsync(string email);
        Task<User> FindByIdOrThrowAsync(string userId);
    }
}