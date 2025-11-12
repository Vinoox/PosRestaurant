using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Common;

namespace Domain.Entities
{
    public class Category : AuditableEntity
    {
        public required string Name { get; set; }

        public int RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; }
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();

        private Category() 
        {
        }

        public static Category Create(string name, int restaurantId)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Category name cannot be empty.", nameof(name));
            
            return new Category
            {
                Name = name.Trim(),
                RestaurantId = restaurantId
            };
        }
    }
}
