using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly PosRestaurantContext _context;

        public ProductRepository(PosRestaurantContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllAsync(int restaurantId)
        {
            return await _context.Products
                .Where(p => p.RestaurantId == restaurantId)
                .Include(p => p.Category)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetAllByCategoryIdAsync(int restaurantId, int categoryId)
        {
            return await _context.Products
                .Where(p => p.RestaurantId == restaurantId && p.CategoryId == categoryId)
                .ToListAsync();
        }

        public async Task<Product?> GetByProductNameAsync(int restaurantId, string productName)
        {
            return await _context.Products
                .FirstOrDefaultAsync(p => p.RestaurantId == restaurantId && p.Name == productName);
        }

        public async Task<Product?> GetByProductIdAsync(int productId)
        {
            return await _context.Products
                .FirstOrDefaultAsync(p => p.Id == productId);
        }

        public void Add(Product product)
        {
            _context.Products.Add(product);
        }

        public void Delete(Product product)
        {
            _context.Products.Remove(product);
        }
    }
}
