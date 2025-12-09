using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Common;
using Domain.Enums;

namespace Domain.Entities
{
    public class Product : AuditableEntity, ITenantEntity
    {
        public string Name { get; private set; } = null!;

        public string Description { get; private set; } = null!;

        public decimal Price { get; private set; }

        public int CategoryId { get; private set; }
        public virtual Category Category { get; private set; } = null!;

        private readonly List<ProductIngredient> _productIngredients = new();
        public IReadOnlyCollection<ProductIngredient> ProductIngredients => _productIngredients.AsReadOnly();
        public int RestaurantId { get; private set; }
        public Restaurant Restaurant { get; private set; } = null!;
        int ITenantEntity.RestaurantId { get => RestaurantId; set => RestaurantId = value; }

        private Product(){}

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
            if(string.IsNullOrWhiteSpace(newName))
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

        public ProductIngredient AddIngredient(Ingredient ingredient, decimal amount, Unit unit)
        {
            if (_productIngredients.Any(pi => pi.IngredientId == ingredient.Id))
                throw new DomainException($"Składnik '{ingredient.Name}' jest już dodany do tego produktu.");

            if (amount < 0)
                throw new DomainException("Ilość składnika nie może być ujemna");

            var newProductIngredient = new ProductIngredient(this.Id, ingredient.Id, amount, unit);

            _productIngredients.Add(newProductIngredient);

            return newProductIngredient;
        }

        public void RemoveIngredient(int ingredientId)
        {
            var ingredientLink = GetIngredientLinkOrThrow(ingredientId);
            _productIngredients.Remove(ingredientLink);
        }

        public void ClearIngredients()
        {
            _productIngredients.Clear();
        }

        public void UpdateIngredientAmount(int ingredientId, decimal newAmount)
        {
            var ingredientLink = GetIngredientLinkOrThrow(ingredientId);
            ingredientLink.UpdateAmount(newAmount);
        }

        public void UpdateIngredientUnit(int ingredientId, Unit newUnit)
        {
            var ingredientLink = GetIngredientLinkOrThrow(ingredientId);
            ingredientLink.UpdateUnit(newUnit);
        }


        private ProductIngredient GetIngredientLinkOrThrow(int ingredientId)
        {
            if (ingredientId <= 0)
                throw new DomainException("Niepoprawne ID składnika");

            var link = _productIngredients.FirstOrDefault(pi => pi.IngredientId == ingredientId);

            if (link == null)
                throw new DomainException("Składni nie występuje w tym produkcie");

            return link;
        }
    }
}
