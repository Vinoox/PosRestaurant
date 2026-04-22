using PosRestaurant.Shared.Entities;
using PosRestaurant.Shared.Exceptions;
using PosRestaurant.Shared.Interfaces;

namespace Catalog.Domain.Entities
{
    public class Category : BaseAuditableEntity, IMultiTenantEntity
    {
        public string Name { get; private set; } = null!;
        public int RestaurantId { get; private set; }

        private readonly List<Product> _products = new();
        public virtual IReadOnlyCollection<Product> Products => _products.AsReadOnly();

        int IMultiTenantEntity.RestaurantId { get => RestaurantId; set => RestaurantId = value; }

        private Category() {}

        public static Category Create(string name, int restaurantId)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Nazwa kategorii nie może być pusta.");
            return new Category
            {
                Name = name.Trim(),
                RestaurantId = restaurantId
            };
        }

        public void UpdateName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new DomainException("Nowa nazwa nie może być pusta.");

            Name = newName.Trim();
        }
    }
}
