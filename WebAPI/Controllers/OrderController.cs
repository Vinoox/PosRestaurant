using Application.Features.Orders.Dtos.Commands;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WebAPI.Filters;

namespace WebAPI.Controllers
{
    [Route("api/restaurants/{restaurantId}/orders")]
    [ApiController]
    //[Authorize]
    //[ServiceFilter(typeof(ValidateRestaurantAccessFilter))]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create a new order")]
        public async Task<IActionResult> CreateOrder([FromRoute] int restaurantId, [FromBody] CreateOrderDto dto)
        {
            var orderId = await _orderService.CreateOrderAsync(restaurantId, dto);

            return CreatedAtAction(
                nameof(GetOrderById),
                new { restaurantId, orderId },
                new { id = orderId }
            );
        }

        [HttpGet("{orderId}")]
        [SwaggerOperation(Summary = "Get order details")]
        public IActionResult GetOrderById([FromRoute] int restaurantId, [FromRoute] int orderId)
        {
            return Ok(new { Message = "Work in progress", OrderId = orderId });
        }
    }
}