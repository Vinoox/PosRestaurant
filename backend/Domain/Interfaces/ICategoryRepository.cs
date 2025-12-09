using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<IEnumerable<Category>> GetAllByRestaurantIdAsync(int restuarantId);
        Task<Category?> GetByIdAsync(int restaurantId, int id);
        Task<Category?> GetByNameAsync(int restaurantId, string name);
    }
}
