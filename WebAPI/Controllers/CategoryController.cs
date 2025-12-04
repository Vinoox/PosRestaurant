using System.Threading.Tasks;
using Application.Features.Categories.Dtos.Commands;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace WebAPI.Controllers
{
    [Route("api/restaurants/{restaurantId}/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;

        public CategoryController(ICategoryService categoryService, IProductService productService)
        {
            _categoryService = categoryService;
            _productService = productService;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get all categories for a specific restaurant")]
        public async Task<IActionResult> GetCategoriesByRestaurantId([FromRoute] int restaurantId)
        {
            var categories = await _categoryService.GetAllByRestaurantIdAsync(restaurantId);
            return Ok(categories);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create a new category")]
        public async Task<IActionResult> CreateCategory([FromRoute] int restaurantId, [FromBody] CreateCategoryDto dto)
        {
            await _categoryService.CreateAsync(restaurantId, dto);

            return Created();
        }

        [HttpPatch("{categoryId}")]
        [SwaggerOperation(Summary = "Update an existing category")]
        public async Task<IActionResult> UpdateCategory([FromRoute] int restaurantId, [FromRoute] int categoryId,[FromBody] UpdateCategoryDto dto)
        {
            await _categoryService.UpdateAsync(restaurantId, categoryId ,dto);
            return NoContent();
        }

        [HttpDelete("{categoryId}")]
        [SwaggerOperation(Summary = "Delete specific category")]
        public async Task<IActionResult> DeleteCategory([FromRoute] int restaurantId, [FromRoute] int categoryId)
        {
            await _categoryService.DeleteAsync(restaurantId, categoryId);
            return NoContent();
        }

        [HttpGet("{categoryId}/products")]
        [SwaggerOperation(Summary = "Get all products from a specific resturant and category")]
        public async Task<IActionResult> GetAllProductsFromCategory([FromRoute] int restaurantId, [FromRoute] int categoryId)
        {
            var products = await _productService.GetAllByCategoryIdAsync(restaurantId, categoryId);
            return Ok(products);
        }

    }
}