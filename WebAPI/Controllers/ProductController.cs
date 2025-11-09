using Application.Features.Products.Dtos;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [SwaggerOperation(Summary = "Get all products (summary view)")]
        [HttpGet]
        public async Task<IActionResult> GetAllProducts([FromQuery] string? name)
        {
            if(string.IsNullOrWhiteSpace(name))
            {
                var products = await _productService.GetAllAsync();
                return Ok(products);
            }
            else
            {
                var foundProducts = await _productService.SearchByNameAsync(name);
                return Ok(foundProducts);
            }
        }

        [SwaggerOperation(Summary = "Get a single product with full details by id")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productService.GetByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [SwaggerOperation(Summary = "Create a new product")]
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto newProduct)
        {
            var createdProduct = await _productService.CreateAsync(newProduct);

            return CreatedAtAction(nameof(GetProductById), new { id = createdProduct.Id }, createdProduct);
        }

        [SwaggerOperation(Summary = "Update an existing product's core data")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductDto updatedProduct)
        {
            await _productService.UpdateAsync(id, updatedProduct);
            return NoContent();
        }

        [SwaggerOperation(Summary = "Delete a product")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await _productService.DeleteAsync(id);
            return NoContent();
        }


        // --- DEDYKOWANE ENDPOINTY DO ZARZĄDZANIA SKŁADNIKAMI ---

        [SwaggerOperation(Summary = "Add an ingredient to a product")]
        [HttpPost("{productId}/ingredients")]
        public async Task<IActionResult> AddIngredientToProduct(int productId, [FromBody] IngredientAmountDto ingredientDto)
        {
            await _productService.AddIngredientToProductAsync(productId, ingredientDto);
            return Ok();
        }

        [SwaggerOperation(Summary = "Update an ingredient's quantity in a product")]
        [HttpPut("{productId}/ingredients/{ingredientId}")]
        public async Task<IActionResult> UpdateIngredientQuantity(int productId, int ingredientId, [FromBody] IngredientAmountDto ingredientAmountDto)
        {
            await _productService.UpdateIngredientQuantityAsync(productId, ingredientId, ingredientAmountDto.Quantity);
            return NoContent();
        }

        [SwaggerOperation(Summary = "Remove an ingredient from a product")]
        [HttpDelete("{productId}/ingredients/{ingredientId}")]
        public async Task<IActionResult> RemoveIngredientFromProduct(int productId, int ingredientId)
        {
            await _productService.RemoveIngredientFromProductAsync(productId, ingredientId);
            return NoContent();
        }
    }
}