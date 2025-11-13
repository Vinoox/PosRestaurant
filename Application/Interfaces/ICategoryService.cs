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
        Task<CategoryDto?> GetByIdAsync(int id);

        Task<Category> FindByIdOrThrowAsync(int id);
        Task AddAsync(CreateCategoryDto newCategory, int restaurantId);
        Task UpdateAsync(UpdateCategoryDto updatedCategory, int restaurantId);
        Task DeleteAsync(int id);
    }
}
