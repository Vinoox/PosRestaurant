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

        [HttpGet("{productId}")]
        [SwaggerOperation(Summary = "Get product by id")]
        public async Task<IActionResult> GetProductById([FromRoute] int restaurantId, [FromRoute] int productId)
        {
            var product = await _productService.GetByIdAsync(restaurantId, productId);
            return Ok(product);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create new product")]
        public async Task<IActionResult> CreateProduct([FromRoute] int restaurantId, [FromBody] CreateProductDto dto)
        {
            var newProductId = await _productService.CreateAsync(restaurantId, dto);

            return CreatedAtAction(nameof(GetProductById), new { restaurantId = restaurantId, productId = newProductId }, new { id = newProductId });
        }

        [HttpDelete("{productId}")]
        [SwaggerOperation(Summary = "Delete product")]
        public async Task<IActionResult> DeleteProduct([FromRoute] int productId)
        {
            await _productService.DeleteAsync(productId);
            return Ok();
        }

        //[HttpGet]
        //public async Task<IActionResult> GetProducts(
        //    [FromRoute] int restaurantId,
        //    [FromQuery] int? categoryId,
        //    [FromQuery] string? categoryName)
        //{
        //    var products = await _productService.GetAllAsync(restaurantId, categoryId, categoryName);
        //    return Ok(products);
        //}


        //[SwaggerOperation(Summary = "Get all products (summary view)")]
        //[HttpGet]
        //public async Task<IActionResult> GetAllProducts([FromQuery] string? name)
        //{
        //    if(string.IsNullOrWhiteSpace(name))
        //    {
        //        var products = await _productService.GetAllAsync();
        //        return Ok(products);
        //    }
        //    else
        //    {
        //        var foundProducts = await _productService.SearchByNameAsync(name);
        //        return Ok(foundProducts);
        //    }
        //}

        //[HttpGet("{productId}")]
        //[SwaggerOperation(Summary = "Get a single product with full details by id")]
        //public async Task<IActionResult> GetProductById([FromRoute] int resturantId, [FromRoute] int productId)
        //{
        //    var product = await _productService.GetByIdAsync(productId);

        //    if (product == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(product);
        //}

        //[HttpPost]
        //[SwaggerOperation(Summary = "Create a new product")]
        //public async Task<IActionResult> CreateProduct([FromRoute] int restaurantId,[FromBody] CreateProductDto dto)
        //{
        //    //var newProduct = await _productService.CreateAsync(dto, restaurantId);

        //    return CreatedAtAction(nameof(GetProductById), new { restaurantId = restaurantId, productId = newProduct.Id }, new { id = newProduct.Id });
        //}

        //[SwaggerOperation(Summary = "Update an existing product's core data")]
        //[HttpPut("{id}")]
        //public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductDto updatedProduct)
        //{
        //    await _productService.UpdateAsync(id, updatedProduct);
        //    return NoContent();
        //}

        //[SwaggerOperation(Summary = "Delete a product")]
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteProduct(int id)
        //{
        //    await _productService.DeleteAsync(id);
        //    return NoContent();
        //}


        //// --- DEDYKOWANE ENDPOINTY DO ZARZĄDZANIA SKŁADNIKAMI ---

        //[SwaggerOperation(Summary = "Add an ingredient to a product")]
        //[HttpPost("{productId}/ingredients")]
        //public async Task<IActionResult> AddIngredientToProduct(int productId, [FromBody] IngredientAmountDto ingredientDto)
        //{
        //    await _productService.AddIngredientToProductAsync(productId, ingredientDto);
        //    return Ok();
        //}

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