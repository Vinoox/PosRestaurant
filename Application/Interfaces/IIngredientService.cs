using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Ingredients.Dtos.Command;
using Application.Features.Ingredients.Dtos.Queries;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IIngredientService
    {
        Task<IEnumerable<IngredientDto>> GetAllAsync(int restaurantId);
        Task<IngredientDto?> GetByIdAsync(int restaurantId, int id);
        Task<IngredientDto?> GetByNameAsync(int restaurantId, string name);
        Task<int> CreateAsync(int restaurantId, CreateIngredientDto newIngredient);
        Task UpdateAsync(int restuarntId, int id, UpdateIngredientDto updatedIngredient);
        Task DeleteAsync(int restaurntId, int id);

        Task<Ingredient> FindByIdOrThrowAsync(int restaurntId, int id);
    }
}
