using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllByRestaurantIdAsync(int restuarantId);
        Task<Category?> GetByIdAsync(int id);
        Task<Category?> GetByNameAsync(string name, int restaurantId);
        void Add(Category category);
        void Update(Category category);
        Task DeleteAsync(int id);
    }
}
