using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<IEnumerable<Product>> GetAllAsync(int restaurantId);
        Task<IEnumerable<Product>> GetAllByCategoryIdAsync(int restaurantId, int categoryId);
        Task<Product?> GetByNameAsync(int restaurantId, string productName);
        Task<Product?> GetByIdAsync(int restaurantId, int productId);
        Task<Product?> GetByIdWithDetailsAsync(int restaurantId, int productId);
    }
}
