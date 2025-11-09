using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class StaffAssignment
    {
        public string UserId { get; private set; } = null!;
        public User User { get; private set; } = null!;

        public int RestaurantId { get; private set; }
        public Restaurant Restaurant { get; private set; } = null!;

        public string RoleId { get; private set; } = null!;
        public IdentityRole Role { get; private set; } = null!;
    
        private StaffAssignment()
        {
        }

        public static StaffAssignment Create(User user, Restaurant restaurant, IdentityRole role)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (restaurant == null)
                throw new ArgumentNullException(nameof(restaurant));
            if (role == null)
                throw new ArgumentNullException(nameof(role));

            return new StaffAssignment
            {
                User = user,
                UserId = user.Id,
                Restaurant = restaurant,
                RestaurantId = restaurant.Id,
                Role = role,
                RoleId = role.Id
            };
        }
    }
}
