using Application.Features.Users.Dtos.Commands;
using Application.Features.Users.Dtos.Queries;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
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