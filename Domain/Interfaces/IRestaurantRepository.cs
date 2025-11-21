using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IRestaurantRepository : IGenericRepository<Restaurant>
    {
        Task<Restaurant?> GetByIdAsync(int id);
        Task<IEnumerable<Restaurant>> FindByUserIdAsync(string userId);
        Task<int> CountByIdAndRoleNameAsync(int id, string roleName);
    }
}
