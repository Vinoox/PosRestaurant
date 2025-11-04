using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Ingredients.Dtos;

namespace Application.Features.Ingredients
{
    public interface IIngredientService
    {
        Task<IEnumerable<IngredientDto>> GetAllAsync();
        Task<IngredientDto?> GetByNameAsync(string name);
        Task<IngredientDto> CreateAsync(CreateIngredientDto newIngredient);
        Task UpdateAsync(int id, UpdateIngredientDto updatedIngredient);
        Task DeleteAsync(int id);
    }
}
