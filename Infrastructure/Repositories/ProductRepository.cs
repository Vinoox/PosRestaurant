using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(PosRestaurantContext context) : base(context) { }

        public async Task<IEnumerable<Product>> GetAllAsync(int restaurantId)
        {
            return await _context.Products
                .Where(p => p.RestaurantId == restaurantId)
                .Include(p => p.Category)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetAllByCategoryIdAsync(int restaurantId, int categoryId)
        {
            return await _context.Products
                .Where(p => p.RestaurantId == restaurantId && p.CategoryId == categoryId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Product?> GetByNameAsync(int restaurantId, string productName)
        {
            return await _context.Products
                .FirstOrDefaultAsync(p => p.RestaurantId == restaurantId && p.Name.ToLower() == productName.ToLower());
        }

        public async Task<Product?> GetByIdAsync(int restaurantId, int productId)
        {
            return await _context.Products
                .FirstOrDefaultAsync(p => p.Id == productId && p.RestaurantId == restaurantId);
        }

        public async Task<Product?> GetByIdWithDetailsAsync(int restaurantId, int productId)
        {
            return await _context.Products
            .Include(p => p.ProductIngredients)
            .Include(p => p.Category)
            .SingleOrDefaultAsync(p => p.Id == productId && p.RestaurantId == restaurantId);
        }
    }
}
