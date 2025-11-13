using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IProductRepository
    {
        //Task<IEnumerable<Product>> GetAllAsync(int restaurantId, int? categoryId);

        Task<IEnumerable<Product>> GetAllAsync(int restaurantId);
        Task<IEnumerable<Product>> GetAllByCategoryIdAsync(int restaurantId, int categoryId);
        Task<Product?> GetByProductNameAsync(int restaurantId, string productName);
        Task<Product?> GetByProductIdAsync(int productId);
        void Add(Product product);
        void Delete(Product product);
        


        //Task<Product?> GetByIdWithDetailsAsync(int restuarantId, int id);
        //void Update(int restuarantId, Product product);
    }
}
