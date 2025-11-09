using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Restaurants.Dtos;
using Application.Features.Restaurants.Dtos.Queries;

namespace Application.Features.Users.Dtos.Queries
{
    public class AuthenticationResultDto
    {
        public string UserId { get; set; } = null!;
        public string AuthenticationToken { get; set; } = null!;
        public IEnumerable<RestaurantSummaryDto> AvailableRestaurants { get; set; } = new List<RestaurantSummaryDto>();
    }
}
