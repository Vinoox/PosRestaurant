using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUserRepository
    {

        Task<IEnumerable<User>> GetAllAsync();

        Task<User?> GetByIdAsync(int id);

        Task<User?> GetByEmailAsync(string email);

        Task<bool> IsEmailUniqueAsync(string email);

        Task<User> AddAsync(User user);

        Task UpdateAsync(User user);

        Task DeleteAsync(int id);
    }
}