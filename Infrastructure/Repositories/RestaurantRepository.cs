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
    public class RestaurantRepository : GenericRepository<Restaurant>, IRestaurantRepository
    {
        public RestaurantRepository(PosRestaurantContext context) : base(context)
        {
        }
        public async Task<Restaurant?> GetByIdAsync(int id)
        {
            return await _context.Restaurants
                .Include(r => r.StaffAssignments).ThenInclude(sa => sa.User)
                .Include(r => r.StaffAssignments).ThenInclude(sa => sa.Role)
                .Include(r => r.Categories).ThenInclude(c => c.Products).ThenInclude(p => p.ProductIngredients)
                .Include(r => r.Ingredients)
                .FirstOrDefaultAsync(r => r.Id == id);
        }
        public async Task<IEnumerable<Restaurant>> FindByUserIdAsync(string userId)
        {
            return await _context.StaffAssignments
                .Where(sa => sa.UserId == userId)
                .Select(sa => sa.Restaurant)
                .ToListAsync();
        }

        public async Task<int> CountByIdAndRoleNameAsync(int id, string roleName)
        {
            return await _context.Restaurants
                .Where(r => r.Id == id && r.StaffAssignments.Any(sa => sa.Role.Name == roleName))
                .CountAsync();
        }
    }
}
