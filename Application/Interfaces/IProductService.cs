using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Products.Dtos;
using Domain.Interfaces;

namespace Application.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<SimpleProductDto>> GetAllAsync();
        Task<IEnumerable<SimpleProductDto>> SearchByNameAsync(string name);
        Task<ProductDto?> GetByIdAsync(int id);
        Task<ProductDto> CreateAsync(CreateProductDto newProduct);
        Task UpdateAsync(int id, UpdateProductDto updatedProduct);
        Task DeleteAsync(int id);

        Task AddIngredientToProductAsync(int productId, IngredientAmountDto ingredientAmountDto);
        Task RemoveIngredientFromProductAsync(int productId, int ingredientId);
        Task UpdateIngredientQuantityAsync(int productId, int ingredientId, int newQuantity);
    }
}
