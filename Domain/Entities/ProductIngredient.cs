using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Common;
using Domain.Enums;

namespace Domain.Entities
{
    public class ProductIngredient
    {
        public int ProductId { get; private set; }
        public Product Product { get; private set; }


        public int IngredientId { get; private set; }
        public Ingredient Ingredient { get; private set; }

        public decimal Amount { get; private set; }
        public Unit Unit { get; private set; }

        private ProductIngredient() { }

        public ProductIngredient(int productId, int ingredientId, decimal amount, Unit unit)
        {
            if (amount <= 0)
                throw new DomainException("Ilość składnika musi być dodatania");

            ProductId = productId;
            IngredientId = ingredientId;
            Amount = amount;
            Unit = unit;
        }

        public void UpdateAmount(decimal newAmount, Unit newUnit)
        {
            if (newAmount <= 0)
                throw new DomainException("Ilość skłądnika musi być dodatnia");

            Amount = newAmount;
            Unit = newUnit;
        }
    }
}
