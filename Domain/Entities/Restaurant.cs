using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Common;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class Restaurant : AuditableEntity
    {
        public string Name { get; private set; } = null!;
        public ICollection<Category> Categories { get; private set; } = new List<Category>();
        public ICollection<Product> Products { get; private set; } = new List<Product>();
        public ICollection<Ingredient> Ingredients { get; private set; } = new List<Ingredient>();
        public ICollection<StaffAssignment> StaffAssignments { get;  set; } = new List<StaffAssignment>();

        //public ICollection<Order> Orders { get; set; } = new List<Order>();

        private Restaurant()
        {
        }

       public static Restaurant Create(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Restaurant name cannot be empty.", nameof(name));
            
            return new Restaurant
            {
                Name = name.Trim()
            };
        }

        public void UpdateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Restaurant name cannot be empty.", nameof(name));
            Name = name.Trim();
        }
    }
}
