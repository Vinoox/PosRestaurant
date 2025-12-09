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
    public class StaffAssignmentRepository : GenericRepository<StaffAssignment>, IStaffAssignmentRepository
    {
        public StaffAssignmentRepository(PosRestaurantContext context) : base(context)
        {
        }
        public async Task<StaffAssignment?> FindByUserIdAndRestaurantIdAsync(string userId, int restaurantId)
        {
            return await _context.StaffAssignments
                .Include(sa => sa.Role)
                .FirstOrDefaultAsync(sa => sa.UserId == userId && sa.RestaurantId == restaurantId);
        }

        public async Task<IEnumerable<StaffAssignment>> GetByRestaurantIdAsync(int restaurantId)
        {
            return await _context.StaffAssignments
                .Include(sa => sa.User)
                .Include(sa => sa.Role)
                .Where(sa => sa.RestaurantId == restaurantId)
                .ToListAsync();
        }

        public async Task<IEnumerable<StaffAssignment>> GetStaffMembersAsync(int restaurantId)
        {
            return await _context.StaffAssignments
                .Include(sa => sa.User)
                .Include(sa => sa.Role)
                .Where(sa => sa.RestaurantId == restaurantId)
                .ToListAsync();
        }
    }
}
