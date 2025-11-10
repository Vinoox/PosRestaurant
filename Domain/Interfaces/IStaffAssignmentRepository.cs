using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IStaffAssignmentRepository
    {
        Task<StaffAssignment?> FindByUserIdAndRestaurantIdAsync(string userId, int restaurantId);
        Task<IEnumerable<StaffAssignment>> GetByRestaurantIdAsync(int restaurantId);
        void Add(StaffAssignment staffAssignment);
        void Remove(StaffAssignment staffAssignment);
    }
}
