using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.StaffManagement.Dtos.Commands;
using Application.Features.StaffManagement.Dtos.Queries;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Application.Interfaces
{
    public interface IStaffManagementService
    {
        Task AddStaffMemberAsync(int restaurantId, AddStaffMemberDto dto);
        Task AddInitialMemberAsync(Restaurant restaurant, User user);
        Task RemoveStaffMemberAsync(int restaurantId, RemoveStaffMemberDto dto);
        Task ChangeStaffMemberRoleAsync(int restaurantId, ChangeStaffMemberRoleDto dto);
        Task<IEnumerable<StaffAssignmentDto>> GetStaffMembersAsync(int restaurantId);
    }
}
