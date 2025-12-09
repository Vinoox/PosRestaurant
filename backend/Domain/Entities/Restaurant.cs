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
        private readonly List<Category> _categories = new();
        public virtual IReadOnlyCollection<Category> Categories => _categories.AsReadOnly();


        private readonly List<Product> _products = new();
        public virtual IReadOnlyCollection<Product> Products => _products.AsReadOnly();


        private readonly List<Ingredient> _ingredients = new();
        public virtual IReadOnlyCollection<Ingredient> Ingredients => _ingredients.AsReadOnly();


        private readonly List<StaffAssignment> _staffAssignments = new();
        public virtual IReadOnlyCollection<StaffAssignment> StaffAssignments => _staffAssignments.AsReadOnly();

        private readonly List<Order> _orders = new();
        public virtual IReadOnlyCollection<Order> Orders => _orders.AsReadOnly();
        private Restaurant(){}

       public static Restaurant Create(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Nazwa nie może być pusta");
            
            return new Restaurant
            {
                Name = name.Trim()
            };
        }

        public void UpdateName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new DomainException("Nazwa nie może być pusta");

            Name = newName.Trim();
        }
    }
}
