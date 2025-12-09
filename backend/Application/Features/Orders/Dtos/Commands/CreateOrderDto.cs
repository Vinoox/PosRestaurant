using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enums;

namespace Application.Features.Orders.Dtos.Commands
{
    public class CreateOrderDto
    {
        public OrderType Type { get; set; }
        public DateTime? TargetCompletionDate { get; set; }
        public string? CustomerName { get; set; }
        public string? PhoneNumber { get; set; }
        public AddressDto? DeliveryAddress { get; set; }
        public string? DriverId { get; set; }
        public List<CreateOrderItemDto> Items { get; set; } = new();
    }

    public class CreateOrderItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string? Note { get; set; }
    }

    public class AddressDto
    {
        public string Street { get; set; } = null!;
        public string? LocalNumber { get; set; }
        public string? City { get; set; }
        public string? PostalCode { get; set; }
    }
}
