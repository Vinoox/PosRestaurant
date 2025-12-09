using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Categories.Dtos.Commands;
using Application.Features.Categories.Dtos.Queries;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategorySummaryDto>> GetAllByRestaurantIdAsync(int restaurantId);
        Task<CategoryDto?> GetByIdAsync(int restaurantId, int id);
        Task<int> CreateAsync(int restaurantId, CreateCategoryDto newCategory);
        Task UpdateAsync(int restaurantId, int id, UpdateCategoryDto updatedCategory);
        Task DeleteAsync(int restaurantId, int id);


        Task<Category> FindByIdOrThrowAsync(int restaurantId, int id);
    }
}
