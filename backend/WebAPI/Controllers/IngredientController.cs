using System.Threading.Tasks;
using Application.Features.Ingredients.Dtos.Command;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace WebAPI.Controllers
{
    [Route("api/restaurants/{restaurantId}/ingredients")]
    [ApiController]
    public class IngredientsController : ControllerBase
    {
        private readonly IIngredientService _ingredientService;

        public IngredientsController(IIngredientService ingredientService)
        {
            _ingredientService = ingredientService;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get all ingredients from a specific restaurant")]
        public async Task<IActionResult> GetAllIngredients([FromRoute] int restaurantId)
        {
            var ingredients = await _ingredientService.GetAllAsync(restaurantId);
            return Ok(ingredients);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get ingredient by id")]
        public async Task<IActionResult> GetIngredientById([FromRoute] int restaurantId, [FromRoute] int id)
        {
            var ingredient = await _ingredientService.GetByIdAsync(restaurantId, id);
            return Ok(ingredient);
        }


        [SwaggerOperation(Summary = "Create a new ingredient")]
        [HttpPost]
        public async Task<IActionResult> CreateIngredient([FromRoute] int restaurantId, CreateIngredientDto dto)
        {
            var newIngredientId = await _ingredientService.CreateAsync(restaurantId, dto);

            return CreatedAtAction(nameof(GetIngredientById), new { restaurantId = restaurantId, id = newIngredientId }, new { id = newIngredientId });
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete an ingredient")]
        public async Task<IActionResult> DeleteIngredient([FromRoute] int restaurantId, [FromRoute] int id)
        {
            await _ingredientService.DeleteAsync(restaurantId, id);
            return NoContent();
        }

        //[SwaggerOperation(Summary = "Update an existing ingredient")]
        //[HttpPut("{id}")]
        //public async Task<IActionResult> UpdateIngredient(int id, UpdateIngredientDto updatedIngredient)
        //{
        //    await _ingredientService.UpdateAsync(id, updatedIngredient);
        //    return NoContent();
        //}
    }
}