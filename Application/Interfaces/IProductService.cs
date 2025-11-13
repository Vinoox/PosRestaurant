using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Products.Dtos;
using Application.Features.Products.Dtos.Commands;
using Application.Features.Products.Dtos.Queries;
using Domain.Interfaces;

namespace Application.Interfaces
{
    public interface IProductService
    {
        Task <IEnumerable<ProductDto>> GetAllAsync(int restaurantId);

        Task<IEnumerable<ProductDto>> GetAllByCategoryIdAsync(int restaurantId, int categoryId);

        Task <ProductDto> GetByIdAsync(int restuarntId, int productId);
        
        Task<int> CreateAsync(int restaurantId, CreateProductDto dto);

        Task DeleteAsync(int productId);






        //Task UpdateAsync(int id, UpdateProductDto updatedProduct);
        //Task AddIngredientToProductAsync(int productId, IngredientAmountDto ingredientAmountDto);
        //Task RemoveIngredientFromProductAsync(int productId, int ingredientId);
        //Task UpdateIngredientQuantityAsync(int productId, int ingredientId, int newQuantity);
    }
}
