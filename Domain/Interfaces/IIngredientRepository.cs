using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IIngredientRepository
    {
        Task<IEnumerable<Ingredient>> GetAllAsync(int restaurantId);
        Task<IEnumerable<Ingredient>> GetAllByProductIdAsync(int restaurantId, int productId);
        Task<Ingredient?> GetByIdAsync(int restaurantId, int id);
        Task<Ingredient?> GetByNameAsync(int restuarantId, string name);
        void Add(Ingredient ingredient);
        void Delete(Ingredient ingredient);

        //Task UpdateAsync(Ingredient ingredient);
        //Task<int> CountExistingAsync(IEnumerable<int> ingredientIds);
    }
}
