using System;
using System.Collections.Generic;
using System.Linq;
using PosRestaurant.Shared.Entities;
using PosRestaurant.Shared.Exceptions;
using PosRestaurant.Shared.Interfaces;

namespace Catalog.Domain.Entities
{
    public class Product : BaseAuditableEntity, IMultiTenantEntity
    {
        public string Name { get; private set; } = null!;
        public string Description { get; private set; } = null!;
        public decimal Price { get; private set; }

        public int CategoryId { get; private set; }
        public virtual Category Category { get; private set; } = null!;

        public int RestaurantId { get; private set; }
        int IMultiTenantEntity.RestaurantId { get => RestaurantId; set => RestaurantId = value; }

        private Product() { }

        public static Product Create(string name, string description, decimal price, int categoryId, int restaurantId)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new DomainException("Nazwa nie może być pusta");
            }

            if (price < 0)
            {
                throw new DomainException("Cena nie może być ujemna");
            }

            if (categoryId <= 0)
            {
                throw new DomainException("Niepoprawne ID kategorii");
            }

            return new Product
            {
                Name = name,
                Description = description,
                Price = price,
                CategoryId = categoryId,
                RestaurantId = restaurantId
            };
        }

        public void UpdateName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new DomainException("Nazwa nie może być pusta");

            Name = newName;
        }

        public void UpdateDescription(string newDescription)
        {
            if (string.IsNullOrWhiteSpace(newDescription))
                throw new DomainException("Opis nie może być pusty");

            Description = newDescription;
        }

        public void UpdateCategory(int categoryId)
        {
            if (categoryId <= 0)
                throw new DomainException("Niepoprawne ID kategorii");

            CategoryId = categoryId;
        }

        public void UpdatePrice(decimal price)
        {
            if (price < 0)
                throw new DomainException("Cena nie może być ujemna");

            Price = price;
        }
    }
}