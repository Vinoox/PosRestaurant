using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IRestaurantRepository
    {
        Task<Restaurant?> GetByIdAsync(int id);
        Task CreateAsync(Restaurant restaurant);
        Task<IEnumerable<Restaurant>> FindByUserIdAsync(string userId);
    }
}
