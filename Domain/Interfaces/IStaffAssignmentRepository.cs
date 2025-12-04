using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IStaffAssignmentRepository : IGenericRepository<StaffAssignment>
    {
        Task<IEnumerable<StaffAssignment>> GetStaffMembersAsync(int restaurantId);
        Task<StaffAssignment?> FindByUserIdAndRestaurantIdAsync(string userId, int restaurantId);
        Task<IEnumerable<StaffAssignment>> GetByRestaurantIdAsync(int restaurantId);
    }
}
