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
    public class StaffAssignmentRepository : IStaffAssignmentRepository
    {
        private readonly PosRestaurantContext _context;
        public StaffAssignmentRepository(PosRestaurantContext context)
        {
            _context = context;
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

        public void Add(StaffAssignment staffAssignment)
        {
            _context.Entry(staffAssignment).State = EntityState.Added;
        }

        public void Remove(StaffAssignment staffAssignment)
        {
            _context.Entry(staffAssignment).State = EntityState.Deleted;
        }
    }
}
