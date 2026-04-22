using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using MediatR;

// UWAGA: Te usingi zadziałają, gdy utworzysz odpowiednie klasy w Catalog.Application
// using Catalog.Application.Products.Queries.GetAllProducts;
// using Catalog.Application.Products.Queries.GetProductById;
// using Catalog.Application.Products.Commands.CreateProduct;
// using Catalog.Application.Products.Commands.UpdateProduct;
// using Catalog.Application.Products.Commands.DeleteProduct;

namespace Catalog.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")] // Automatycznie: api/v1/products
    // [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get all products for a specific restaurant")]
        public async Task<IActionResult> GetAll([FromQuery] int restaurantId)
        {
            // Docelowo: wyciągnięcie RestaurantId z tokena JWT kelnera
            /*
             * var query = new GetAllProductsQuery { RestaurantId = restaurantId };
             * var products = await _mediator.Send(query);
             * return Ok(products);
             */
            return StatusCode(501, "Endpoint w trakcie migracji na CQRS");
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get product by id")]
        public async Task<IActionResult> GetById([FromRoute] int id, [FromQuery] int restaurantId)
        {
            /*
             * var query = new GetProductByIdQuery { Id = id, RestaurantId = restaurantId };
             * var product = await _mediator.Send(query);
             * return Ok(product);
             */
            return StatusCode(501, "Endpoint w trakcie migracji na CQRS");
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create a new product")]
        public async Task<IActionResult> Create([FromBody] object command) // Zamień 'object' na 'CreateProductCommand'
        {
            /*
             * // command.RestaurantId = restaurantIdFromToken;
             * var newProductId = await _mediator.Send(command);
             * * // Zwracamy status 201 i wskazujemy URI do nowo utworzonego zasobu
             * return CreatedAtAction(nameof(GetById), new { id = newProductId }, new { id = newProductId });
             */
            return StatusCode(501, "Endpoint w trakcie migracji na CQRS");
        }

        [HttpPatch("{id}")]
        [SwaggerOperation(Summary = "Update product details")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] object command) // Zamień na UpdateProductCommand
        {
            /*
             * // command.Id = id;
             * // command.RestaurantId = restaurantIdFromToken;
             * await _mediator.Send(command);
             * return NoContent(); // Status 204
             */
            return StatusCode(501, "Endpoint w trakcie migracji na CQRS");
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete product")]
        public async Task<IActionResult> Delete([FromRoute] int id, [FromQuery] int restaurantId)
        {
            /*
             * var command = new DeleteProductCommand { Id = id, RestaurantId = restaurantId };
             * await _mediator.Send(command);
             * return NoContent(); // Status 204
             */
            return StatusCode(501, "Endpoint w trakcie migracji na CQRS");
        }
    }
}