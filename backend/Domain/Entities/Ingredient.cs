using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Common;
using Domain.Enums;

namespace Domain.Entities
{
    public class Ingredient : AuditableEntity, ITenantEntity
    {
        public string Name { get; private set; } = null!;
        public Unit Unit { get; private set; }
        public int RestaurantId { get; private set; }
        public Restaurant Restaurant { get; private set; } = null!;

        int ITenantEntity.RestaurantId { get => RestaurantId; set => RestaurantId = value; }

        private Ingredient() { }

        public static Ingredient Create(string name, Unit unit, int restaurantId)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Nazwa składnika nie może być pusta");

            if (!Enum.IsDefined(typeof(Unit), unit))
                throw new DomainException("Nieprawidłowa jednostka");

            return new Ingredient()
            {
                Name = name.Trim(),
                Unit = unit,
                RestaurantId = restaurantId
            };
        }
        public void UpdateName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new DomainException("Nazwa składnika nie może być pusta");

            Name = newName.Trim();
        }
        public void UpdateUnit(Unit newUnit)
        {
            if (!Enum.IsDefined(typeof(Unit), newUnit))
                throw new DomainException("Nieprawidłowa jednostka miary.");

            if (newUnit == this.Unit) return;

            Unit = newUnit;
        }
    }
}