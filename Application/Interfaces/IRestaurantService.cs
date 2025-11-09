using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Restaurants.Dtos;
using Application.Features.Restaurants.Dtos.Commands;
using Application.Features.Restaurants.Dtos.Queries;

namespace Application.Interfaces
{
    public interface IRestaurantService
    {
        Task<int> CreateAsync(CreateRestaurantDto dto, string creatorUserId);

        Task<RestaurantDto?> GetByIdAsync(int id);

        Task<IEnumerable<RestaurantSummaryDto>> GetByUserIdAsync(string userId);

        Task AddStaffMemberAsync(int id, AddStaffMemberDto dto);
    }
}
