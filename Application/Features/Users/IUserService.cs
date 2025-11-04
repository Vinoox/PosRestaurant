using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Users.Dtos;

namespace Application.Features.Users
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllAsync();
        Task<UserDto?> GetByIdAsync(int id);
        Task<UserDto> RegisterAsync(RegisterUserDto newUser);
        Task UpdateAsync(int id, UpdateUserDto updatedUser);
        Task DeleteAsync(int id);
    }
}
