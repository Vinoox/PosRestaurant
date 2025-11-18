using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Products.Dtos;
using Application.Features.Products.Dtos.Commands;
using Application.Features.Products.Dtos.Queries;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Interfaces
{
    public interface IProductService
    {
        Task <IEnumerable<ProductDto>> GetAllAsync(int restaurantId);
        Task<IEnumerable<ProductDto>> GetAllByCategoryIdAsync(int restaurantId, int categoryId);
        Task <ProductDto> GetByIdAsync(int restuarntId, int productId);
        Task<int> CreateAsync(int restaurantId, CreateProductDto dto);
        Task UpdateDetailsAsync(int restaurantId, int productId, UpdateProductDto dto);
        Task DeleteAsync(int restaurantId, int productId);

        Task<Product> FindByIdOrThrowAsync(int restaurantId, int productId);


        Task<ProductIngredientDto> AddIngredientToProductAsync(int restaurantId, int productId, AddIngredientToProductDto dto);
        Task RemoveIngredientFromProductAsync(int restaurantId, int productId, int ingredientId);
        Task UpdateIngredientQuantityAsync(int restaurantId, int productId, int ingredientId, int newQuantity);
    }
}
