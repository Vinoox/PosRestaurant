using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Common;

namespace Domain.Entities
{
    public class OrderItem : AuditableEntity
    {

        public int OrderId { get; private set; }
        public Order Order { get; private set; } = null!;

        public int ProductId { get; private set; }
        public Product Product { get; private set; } = null!;

        public string ProductName { get; private set; } = null!;
        public decimal UnitPrice { get; private set; }

        public int Quantity { get; private set; }

        public decimal TotalPrice => UnitPrice * Quantity;

        private OrderItem() { }

        internal OrderItem(int productId, string productName, decimal unitPrice, int quantity)
        {
            if (quantity <= 0)
                throw new DomainException("Ilość w zamówieniu musi być dodatnia.");

            if (unitPrice < 0)
                throw new DomainException("Cena nie może być ujemna.");

            ProductId = productId;
            ProductName = productName;
            UnitPrice = unitPrice;
            Quantity = quantity;
        }

        public void IncreaseQuantity(int amount)
        {
            if (amount <= 0)
                throw new DomainException("Ilość do zwiększenia musi być dodatnia.");
            Quantity += amount;
        }
    }
}
