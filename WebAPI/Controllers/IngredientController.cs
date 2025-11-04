using System.Threading.Tasks;
using Application.Features.Ingredients;
using Application.Features.Ingredients.Dtos;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngredientsController : ControllerBase
    {
        private readonly IIngredientService _ingredientService;

        public IngredientsController(IIngredientService ingredientService)
        {
            _ingredientService = ingredientService;
        }

        [SwaggerOperation(Summary = "Get all ingredients")]
        [HttpGet]
        public async Task<IActionResult> GetAllIngredients()
        {
            var ingredients = await _ingredientService.GetAllAsync();
            return Ok(ingredients);
        }

        [SwaggerOperation(Summary = "Create a new ingredient")]
        [HttpPost]
        public async Task<IActionResult> CreateIngredient(CreateIngredientDto newIngredient)
        {
            var createdCategory = await _ingredientService.CreateAsync(newIngredient);

            var resourceUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{HttpContext.Request.Path}";

            var locationUrl = $"{resourceUrl}/{createdCategory.Id}";
            return Created(locationUrl, createdCategory);
        }

        [SwaggerOperation(Summary = "Update an existing ingredient")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateIngredient(int id, UpdateIngredientDto updatedIngredient)
        {
            await _ingredientService.UpdateAsync(id, updatedIngredient);
            return NoContent();
        }

        [SwaggerOperation(Summary = "Delete an ingredient")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIngredient(int id)
        {
            await _ingredientService.DeleteAsync(id);
            return NoContent();
        }
    }
}