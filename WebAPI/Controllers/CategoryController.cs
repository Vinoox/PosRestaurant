using System.Threading.Tasks;
using Application.Features.Categories.Dtos.Commands;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace WebAPI.Controllers
{
    [Route("api/{restaurantId}/categories")]
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
        public async Task<IActionResult> CreateCategory([FromRoute] int restaurantId, [FromBody]CreateCategoryDto dto)
        {
            await _categoryService.AddAsync(dto, restaurantId);

            return Created();
        }

        [HttpPatch]
        [SwaggerOperation(Summary = "Update an existing category")]
        public async Task<IActionResult> UpdateCategory([FromRoute] int restaurantId, [FromBody] UpdateCategoryDto dto)
        {
            await _categoryService.UpdateAsync(dto, restaurantId);
            return NoContent();
        }

        [HttpGet("{categoryId}/products")]
        [SwaggerOperation(Summary = "Get all products from a specific resturant and category")]
        public async Task<IActionResult> GetAllProductsFromCategory([FromRoute] int restaurantId, [FromRoute] int categoryId)
        {
            var products = await _productService.GetAllByCategoryIdAsync(restaurantId, categoryId);
            return Ok(products);
        }



        //[SwaggerOperation(Summary = "Get category by id")]
        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetCategoryById(int id)
        //{
        //    var category = await _categoryService.GetByIdAsync(id);
        //    if (category == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(category);
        //}

        //[SwaggerOperation(Summary = "Get all categories")]
        //[HttpGet]
        //public async Task<IActionResult> GetAllCategories()
        //{
        //    var categories = await _categoryService.GetAllAsync();
        //    return Ok(categories);
        //}

        //[SwaggerOperation(Summary = "Update an existing category")]
        //[HttpPut("{id}")]
        //public async Task<IActionResult> UpdateCategory(int id, UpdateCategoryDto updatedCategory)
        //{
        //    await _categoryService.UpdateAsync(id, updatedCategory);
        //    return NoContent();
        //}

        //[SwaggerOperation(Summary = "Delete a category")]
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteCategory(int id)
        //{
        //    await _categoryService.DeleteAsync(id);
        //    return NoContent();
        //}
    }
}