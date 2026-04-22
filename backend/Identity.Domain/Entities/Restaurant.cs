using PosRestaurant.Shared.Entities;
using PosRestaurant.Shared.Exceptions;

namespace Identity.Domain.Entities
{
    public class Restaurant : BaseAuditableEntity
    {
        public string Name { get; private set; } = null!;

        public string? Address { get; private set; }
        public string? TaxId { get; private set; }

        private Restaurant() { }

        public static Restaurant Create(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Nazwa restauracji nie może być pusta.");

            return new Restaurant { Name = name.Trim() };
        }

        public void UpdateDetails(string name, string address)
        {
            Name = name.Trim();
            Address = address;
        }
    }
}