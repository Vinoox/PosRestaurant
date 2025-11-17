using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Common;

namespace Domain.Entities
{
    public class Category : AuditableEntity, ITenantEntity
    {
        public string Name { get; private set; } = null!;
        public int RestaurantId { get; set; }
        public Restaurant Restaurant { get; private set; } = null!;
        public ICollection<Product> Products { get; private set; } = new List<Product>();

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

        public void UpdateName(string newName)
        {
            if (!string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("Nazwa kategorii nie może być pusta.", nameof(newName));

            Name = newName;
        }
    }
}
