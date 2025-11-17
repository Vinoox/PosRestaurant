using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using Application.Features.Products.Dtos;
using Application.Features.Products.Dtos.Commands;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace WebAPI.Controllers
{
    [Route("api/restaurants/{restaurantId}/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get all products from a specific restaurant")]
        public async Task<IActionResult> GetAllProducts([FromRoute] int restaurantId)
        {
            var products = await _productService.GetAllAsync(restaurantId);
            return Ok(products);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get product by id")]
        public async Task<IActionResult> GetProductById([FromRoute] int restaurantId, [FromRoute] int id)
        {
            var product = await _productService.GetByIdAsync(restaurantId, id);
            return Ok(product);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create new product")]
        public async Task<IActionResult> CreateProduct([FromRoute] int restaurantId, [FromBody] CreateProductDto dto)
        {
            var newProductId = await _productService.CreateAsync(restaurantId, dto);

            return CreatedAtAction(nameof(GetProductById), new { restaurantId = restaurantId, id = newProductId }, new { id = newProductId });
        }

        [HttpDelete("{productId}")]
        [SwaggerOperation(Summary = "Delete product")]
        public async Task<IActionResult> DeleteProduct([FromRoute] int restaurantId, [FromRoute] int productId)
        {
            await _productService.DeleteAsync(restaurantId, productId);
            return NoContent();
        }



        [HttpPost("{productId}/ingredients")]
        [SwaggerOperation(Summary = "Add ingredient to specific product")]
        public async Task<IActionResult> AddIngredientProduct([FromRoute] int restaurantId, [FromRoute] int productId, AddIngredientToProductDto dto)
        {
            var addedIngredient = await _productService.AddIngredientToProductAsync(restaurantId, productId, dto);
            return Ok(addedIngredient);
        }



        //[SwaggerOperation(Summary = "Update an ingredient's quantity in a product")]
        //[HttpPut("{productId}/ingredients/{ingredientId}")]
        //public async Task<IActionResult> UpdateIngredientQuantity(int productId, int ingredientId, [FromBody] IngredientAmountDto ingredientAmountDto)
        //{
        //    await _productService.UpdateIngredientQuantityAsync(productId, ingredientId, ingredientAmountDto.Quantity);
        //    return NoContent();
        //}

        //[SwaggerOperation(Summary = "Remove an ingredient from a product")]
        //[HttpDelete("{productId}/ingredients/{ingredientId}")]
        //public async Task<IActionResult> RemoveIngredientFromProduct(int productId, int ingredientId)
        //{
        //    await _productService.RemoveIngredientFromProductAsync(productId, ingredientId);
        //    return NoContent();
        //}
    }
}