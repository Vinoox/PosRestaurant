using Catalog.Domain.Entities;
using PosRestaurant.Shared.Interfaces;

namespace Catalog.Domain.Interfaces
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<IEnumerable<Category>> GetAllByRestaurantIdAsync(int restaurantId);
        Task<Category?> GetByIdAsync(int restaurantId, int id);
        Task<Category?> GetByNameAsync(int restaurantId, string name);
    }
}
