using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Domain.Common;
using Domain.Enums;

namespace Domain.Entities
{
    public class Order : AuditableEntity, ITenantEntity
    {
        public int DailySequenceNumber { get; private set; }

        public int RestaurantId { get; private set; }
        public Restaurant Restaurant { get; private set; } = null!;
        int ITenantEntity.RestaurantId { get => RestaurantId; set => RestaurantId = value; }

        public DateTime OrderDate { get; private set; }
        public DateTime? TargetCompletionDate { get; private set; }

        public OrderType Type { get; private set; }
        public OrderStatus Status { get; private set; }

        public string? CustomerName { get; private set; }
        public string? PhoneNumber { get; private set; }
        public Address? DeliveryAddress { get; private set; }

        public int? DriverId { get; private set; }
        public virtual StaffAssignment? Driver { get; private set; }

        private readonly List<OrderItem> _items = new();
        public virtual IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

        public decimal TotalAmount => _items.Sum(i => i.TotalPrice);

        private Order() { }
        public static Order Create(
            int restaurantId,
            int dailySequenceNumber,
            OrderType type,
            DateTime targetDate,
            string? customerName,
            string? phoneNumber,
            Address? address)
        {
            if (dailySequenceNumber <= 0)
                throw new DomainException("Niepoprawny numer zamówienia");

            if(type != OrderType.DineIn)
            {
                if (string.IsNullOrWhiteSpace(customerName))
                    throw new DomainException("Imię klienta jest wymagane");
                if (string.IsNullOrWhiteSpace(phoneNumber))
                    throw new DomainException("Numer telefonu jest wymagany");

                if (type == OrderType.Delivery && address == null)
                    throw new DomainException("Adres dostawy jest wymagany dla zamówień na dowóz");

            }

            return new Order
            {
                RestaurantId = restaurantId,
                DailySequenceNumber = dailySequenceNumber,
                OrderDate = DateTime.UtcNow,
                TargetCompletionDate = targetDate,
                Type = type,
                Status = OrderStatus.Pending,
                CustomerName = customerName,
                PhoneNumber = phoneNumber
            };
        }

        public void AddItem(Product product, int quantity)
        {
            if (product == null)
                throw new ArgumentNullException("Produkt nie istnieje");


            var existingItem = _items.FirstOrDefault(i => i.ProductId == product.Id);
            if (existingItem != null)
            {
                existingItem.IncreaseQuantity(quantity);
                return;
            }


            var orderItem = new OrderItem(product.Id, product.Name, product.Price, quantity);
            _items.Add(orderItem);
        }

        public void AssignDriver(StaffAssignment driver)
        {
            if (Type != OrderType.Delivery)
                throw new DomainException("Nie można przypisać kierowcy do tego zamówienia");

            if (driver == null)
                throw new ArgumentNullException("Przypisany pracownik nie istnieje");

            if (driver.Role.Name != "Driver")
                throw new DomainException("Przypisany pracownik nie jest kierowcą");

            DriverId = driver.Id;

            Status = OrderStatus.InDelivery;
        }

        public void MarkAsInProgress()
        {
            if (Status != OrderStatus.Pending)
                throw new DomainException("Zamówienie nie jest w statusie oczekującym");

            Status = OrderStatus.InProgress;
        }

        public void MarkAsReady()
        {
            if (Status != OrderStatus.InProgress)
                throw new DomainException("Zamówienie nie jest w statusie w trakcie realizacji");

            Status = OrderStatus.Ready;
        }

        public class Address
        {
            public string Street { get; private set; } = null!;
            public string? LocalNumber { get; private set; }
            public string? City { get; private set; }
            public string? PostalCode { get; private set; }
            private Address() { }
            public Address Create(string street, string? LocalNumber, string? city, string? postalCode)
            {
                if (string.IsNullOrWhiteSpace(street))
                    throw new DomainException("Ulica jest wymagana");

                return new Address
                {
                    Street = street,
                    LocalNumber = LocalNumber,
                    City = city,
                    PostalCode = postalCode
                };
            }
        }

    }
}
