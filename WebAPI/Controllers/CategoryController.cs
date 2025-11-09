using System.Threading.Tasks;
using Application.Features.Categories.Dtos;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [SwaggerOperation(Summary = "Get all categories")]
        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _categoryService.GetAllAsync();
            return Ok(categories);
        }

        [SwaggerOperation(Summary = "Get category by id")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }

        [SwaggerOperation(Summary = "Create a new category")]
        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategoryDto newCategory)
        {
            var createdCategory = await _categoryService.AddAsync(newCategory);

            return Created($"api/users/{createdCategory.Id}", createdCategory);
        }

        [SwaggerOperation(Summary = "Update an existing category")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, UpdateCategoryDto updatedCategory)
        {
            await _categoryService.UpdateAsync(id, updatedCategory);
            return NoContent();
        }

        [SwaggerOperation(Summary = "Delete a category")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            await _categoryService.DeleteAsync(id);
            return NoContent();
        }
    }
}