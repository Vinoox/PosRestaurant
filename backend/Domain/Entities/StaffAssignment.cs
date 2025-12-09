using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Common;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class StaffAssignment : AuditableEntity
    {
        public string UserId { get;  private set; } = null!;
        public User User { get;  private set; } = null!;

        public int RestaurantId { get;  private set; }
        public Restaurant Restaurant { get;  private set; } = null!;

        public string RoleId { get;  private set; } = null!;
        public IdentityRole Role { get;  private set; } = null!;

        private StaffAssignment()
        {
        }

        public static StaffAssignment Create(User user, Restaurant restaurant, IdentityRole role)
        {
            if (user == null) throw new DomainException("Restauracja nie istnieje");
            if (restaurant == null) throw new DomainException("Użytkownik nie istnieje");
            if (role == null) throw new DomainException("Niepoprawna rola");

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

        public void ChangeRole(IdentityRole newRole)
        {
            if (newRole == null) throw new DomainException("Niepoprawna rola");
            if (RoleId == newRole.Id) return;

            Role = newRole;
            RoleId = newRole.Id;
        }
    }
}
