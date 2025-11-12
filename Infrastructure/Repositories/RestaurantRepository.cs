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
    public class RestaurantRepository : IRestaurantRepository
    {
        private readonly PosRestaurantContext _context;
        public RestaurantRepository(PosRestaurantContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Restaurant restaurant)
        {
            await _context.Restaurants.AddAsync(restaurant);
        }
        public async Task<Restaurant?> GetByIdAsync(int id)
        {
            return await _context.Restaurants
                .Include(r => r.StaffAssignments).ThenInclude(sa => sa.User)
                .Include(r => r.StaffAssignments).ThenInclude(sa => sa.Role)
                .AsNoTracking()
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
