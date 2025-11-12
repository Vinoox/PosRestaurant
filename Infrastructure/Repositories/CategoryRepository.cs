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
    public class CategoryRepository : ICategoryRepository
    {
        private readonly PosRestaurantContext _context;

        public CategoryRepository(PosRestaurantContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllByRestaurantIdAsync(int restaurantId)
        {
            return await _context.Categories
                .Where(c => c.RestaurantId == restaurantId)
                .ToListAsync();
            //return await _context.Categories.ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task<Category?> GetByNameAsync(string name, int restaurantId)
        {
            var normalizedName = name.ToLower();
            
            return await _context.Categories
                .Where(c => c.RestaurantId == restaurantId)
                .FirstOrDefaultAsync(c => c.Name.ToLower() == normalizedName);
        }

        public void Add(Category category)
        {
            _context.Categories.Add(category);
        }

        public async Task DeleteAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }

        public void Update(Category category)
        {
            _context.Categories.Update(category);
        }
    }
}
