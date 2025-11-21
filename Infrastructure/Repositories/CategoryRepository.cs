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
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(PosRestaurantContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Category>> GetAllByRestaurantIdAsync(int restaurantId)
        {
            return await _context.Categories
                .Where(c => c.RestaurantId == restaurantId)
                .ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(int restaurantId, int id)
        {
            return await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == id && c.RestaurantId == restaurantId);
        }

        public async Task<Category?> GetByNameAsync(int restaurantId, string name)
        {
            var normalizedName = name.ToLower();
            
            return await _context.Categories
                .Where(c => c.RestaurantId == restaurantId)
                .FirstOrDefaultAsync(c => c.Name.ToLower() == normalizedName);
        }
    }
}
