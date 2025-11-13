using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Common;

namespace Domain.Entities
{
    public class Product : AuditableEntity
    {
        public string Name { get; private set; }

        public string? Description { get; private set; }

        public decimal Price { get; private set; }

        public int CategoryId { get; private set; }
        public virtual Category Category { get; private set; } = null!;

        private readonly List<ProductIngredient> _productIngredients = [];
        public IReadOnlyCollection<ProductIngredient> ProductIngredients => _productIngredients.AsReadOnly();

        public int RestaurantId { get; private set; }
        public Restaurant Restaurant { get; private set; } = null!;

        private Product()
        {
        }

        public static Product Create(string name, string description, decimal price, int categoryId, int restaurantId)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Product name cannot be empty.", nameof(name));
            }

            if (price < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(price), "Product price must be greater than zero.");
            }

            if (categoryId <= 0)
            {
                throw new ArgumentException(nameof(CategoryId), "Category ID must be greater than zero.");
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

        public void Update(ProductUpdateParams parameters)
        {
            if (!string.IsNullOrWhiteSpace(parameters.Name))
                Name = parameters.Name;

            Description = parameters.Description ?? Description;

            if (parameters.Price.HasValue)
                UpdatePrice(parameters.Price.Value);

            if (parameters.CategoryId.HasValue)
                ChangeCategory(parameters.CategoryId.Value);
        }

        public void UpdatePrice(decimal newPrice)
        {
            if (newPrice <= 0)
                throw new ArgumentOutOfRangeException(nameof(newPrice), "Product price must be greater than zero.");

            Price = newPrice;
        }

        public void ChangeCategory(int newCategoryId)
        {
            if (newCategoryId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(newCategoryId), "Category ID must be greater than zero.");
            }
            CategoryId = newCategoryId;
        }

        public void AddIngredient(int ingredientId, int quantity)
        {
            if (ingredientId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(ingredientId), "Ingredient ID must be greater than zero.");
            }
            if (quantity <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be greater than zero.");
            }

            if (_productIngredients.Any(pi => pi.IngredientId == ingredientId))
            {
                throw new InvalidOperationException("Ingredient already exists in the product.");
            }

            var ingredient = new ProductIngredient {IngredientId = ingredientId, Quantity = quantity};
            _productIngredients.Add(ingredient);
        }

        public void RemoveIngredient(int ingredientId)
        {
            if (ingredientId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(ingredientId), "Ingredient ID must be greater than zero.");
            }

            var ingredient = _productIngredients.FirstOrDefault(pi => pi.IngredientId == ingredientId) ?? throw new InvalidOperationException("Ingredient not found in the product.");
            _productIngredients.Remove(ingredient);
        }

        public void ClearIngredients()
        {
            _productIngredients.Clear();
        }

        public void UpdateIngredientQuantity(int ingredientId, int newQuantity)
        {
            if (newQuantity <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(newQuantity), "Quantity must be greater than zero.");
            }

            if (ingredientId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(ingredientId), "Ingredient ID must be greater than zero.");
            }

            var ingredient = _productIngredients.FirstOrDefault(pi => pi.IngredientId == ingredientId) ?? throw new InvalidOperationException("Ingredient not found in the product.");
            ingredient.Quantity = newQuantity;
        }
    }


    public class ProductUpdateParams
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public int? CategoryId { get; set; }
    }
}
