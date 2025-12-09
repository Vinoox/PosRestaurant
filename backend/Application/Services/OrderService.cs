using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Features.Orders.Dtos.Commands;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using FluentValidation;

namespace Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IStaffAssignmentRepository _staffAssignmentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateOrderDto> _createOrderValidator;

        public OrderService(
            IOrderRepository orderRepository,
            IProductRepository productRepository,
            IStaffAssignmentRepository staffAssignmentRepository,
            IUnitOfWork unitOfWork,
            IValidator<CreateOrderDto> createOrderValidator)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _staffAssignmentRepository = staffAssignmentRepository;
            _unitOfWork = unitOfWork;
            _createOrderValidator = createOrderValidator;
        }

        public async Task<int> CreateOrderAsync(int restaurantId, CreateOrderDto dto)
        {
            await _createOrderValidator.ValidateAndThrowAsync(dto);

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                int dailyNumber = await _orderRepository.GetNextDailySequenceNumberAsync(restaurantId, DateTime.UtcNow);
                Order.Address? deliveryAddress = null;
                if (dto.Type == OrderType.Delivery && dto.DeliveryAddress is not null)
                {
                    deliveryAddress = Order.Address.Create(
                        dto.DeliveryAddress.Street,
                        dto.DeliveryAddress.LocalNumber,
                        dto.DeliveryAddress.City,
                        dto.DeliveryAddress.PostalCode);
                }

                var order = Order.Create(
                    restaurantId,
                    dailyNumber,
                    dto.Type,
                    dto.TargetCompletionDate,
                    dto.CustomerName,
                    dto.PhoneNumber,
                    deliveryAddress
                );

                if (dto.Type == OrderType.Delivery && !string.IsNullOrEmpty(dto.DriverId))
                {
                    var driver = await _staffAssignmentRepository.FindByUserIdAndRestaurantIdAsync(dto.DriverId, restaurantId);

                    if (driver == null)
                    {
                        throw new BadRequestException("Wskazany kierowca nie istnieje w tej restauracji.");
                    }

                    order.AssignDriver(driver);
                }

                foreach (var itemDto in dto.Items)
                {
                    var product = await _productRepository.GetByIdAsync(restaurantId, itemDto.ProductId);

                    if (product == null)
                    {
                        throw new BadRequestException($"Produkt o ID {itemDto.ProductId} nie istnieje w tej restauracji.");
                    }

                    order.AddItem(product, itemDto.Quantity);
                }

                _orderRepository.Add(order);
                await _unitOfWork.CommitTransactionAsync();
                return order.Id;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
    }
}
