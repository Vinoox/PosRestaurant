using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string? UserId => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

        public int? RestaurantId
        {
            get
            {
                var restaurantIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirstValue("restaurantId");
                if (int.TryParse(restaurantIdClaim, out var id))
                {
                    return id;
                }
                return null;
            }
        }
    }
}
