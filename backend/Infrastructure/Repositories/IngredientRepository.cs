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
    public class IngredientRepository : GenericRepository<Ingredient>, IIngredientRepository
    {
        public IngredientRepository(PosRestaurantContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Ingredient>> GetAllAsync(int restaurantId)
        {
            return await _context.Ingredients
                .Where(i => i.RestaurantId == restaurantId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Ingredient>> GetAllByProductIdAsync(int restauarntId, int productId)
        {
            return await _context.ProductIngredients
                .Where(pi => pi.ProductId == productId)
                .Select(pi => pi.Ingredient)
                .Where(pi => pi.RestaurantId == restauarntId)
                .ToListAsync();
        }

        public async Task<Ingredient?> GetByIdAsync(int restaurntId, int ingredientId)
        {
            return await _context.Ingredients
                .FirstOrDefaultAsync(i => i.Id == ingredientId && i.RestaurantId == restaurntId);
        }

        public async Task<Ingredient?> GetByNameAsync(int restaurantId, string name)
        {
            var normalizeName = name.ToLower();

            return await _context.Ingredients
                .Where(i => i.RestaurantId == restaurantId)
                .FirstOrDefaultAsync(i => i.Name.ToLower() == normalizeName);
        }
    }
}
