using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Products.Dtos;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using FluentValidation;

namespace Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IIngredientRepository _ingredientRepository;
        private readonly IMapper _mapper;

        public ProductService(
            IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IIngredientRepository ingredientRepository,
            IMapper mapper)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _ingredientRepository = ingredientRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SimpleProductDto>> GetAllAsync()
        {
            var products = await _productRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<SimpleProductDto>>(products);
        }

        public async Task<ProductDto?> GetByIdAsync(int id)
        {
            var product = await GetProductAndValidateExistenceAsync(id, true);

            return product != null ? _mapper.Map<ProductDto>(product) : null;
        }

        public async Task<IEnumerable<SimpleProductDto>> SearchByNameAsync(string name)
        {
            var products = await _productRepository.SearchByNameAsync(name);

            return _mapper.Map<IEnumerable<SimpleProductDto>>(products);
        }

        public async Task<ProductDto> CreateAsync(CreateProductDto newProduct)
        {
            var categoryExists = await _categoryRepository.GetByIdAsync(newProduct.CategoryId);
            
            if (categoryExists == null)
            {
                throw new ValidationException($"Category with ID {newProduct.CategoryId} does not exist.");
            }

            if(newProduct.Ingredients != null && newProduct.Ingredients.Any())
            {
                var ingredientIds = newProduct.Ingredients.Select(i => i.IngredientId).Distinct().ToList();
                var existingIngredientsCount = await _ingredientRepository.CountExistingAsync(ingredientIds);
                if (existingIngredientsCount != ingredientIds.Count())
                {
                    throw new KeyNotFoundException("One or more ingredients do not exist in the database.");
                }
            }

            var product = Product.Create(
                newProduct.Name,
                newProduct.Description,
                newProduct.Price,
                newProduct.CategoryId
            );

            foreach (var ingredientDto in newProduct.Ingredients)
            {
                product.AddIngredient(ingredientDto.IngredientId, ingredientDto.Quantity);
            }

            await _productRepository.AddAsync(product);

            var productWithDetails = await _productRepository.GetByIdWithDetailsAsync(product.Id);

            return _mapper.Map<ProductDto>(productWithDetails!);
        }

        public async Task UpdateAsync(int id, UpdateProductDto updatedProduct)
        {
            var product = await GetProductAndValidateExistenceAsync(id, true);

            if (updatedProduct.CategoryId.HasValue)
            {
                var category = await _categoryRepository.GetByIdAsync(updatedProduct.CategoryId.Value);
                if (category == null)
                {
                    throw new KeyNotFoundException($"Category with ID {updatedProduct.CategoryId} does not exist.");
                }
            }

            var updateParams = _mapper.Map<ProductUpdateParams>(updatedProduct);

            product.Update(updateParams);

            await _productRepository.UpdateAsync(product);
        }

        public async Task DeleteAsync(int id)
        {
            await _productRepository.DeleteAsync(id);
        }

        public async Task AddIngredientToProductAsync(int productId, IngredientAmountDto ingredientAmountDto)
        {
            var product = await GetProductAndValidateExistenceAsync(productId, true);

            var ingredientExists = await _ingredientRepository.GetByIdAsync(ingredientAmountDto.IngredientId);

            if (ingredientExists == null)
            {
                throw new KeyNotFoundException($"Ingredient with ID {ingredientAmountDto.IngredientId} does not exist.");
            }

            product.AddIngredient(ingredientAmountDto.IngredientId, ingredientAmountDto.Quantity);
            await _productRepository.UpdateAsync(product);
        }

        public async Task RemoveIngredientFromProductAsync(int productId, int ingredientId)
        {
            var product = await GetProductAndValidateExistenceAsync(productId, true);

            var ingredient = await _ingredientRepository.GetByIdAsync(ingredientId);

            if (ingredient == null)
            {
                throw new KeyNotFoundException($"Ingredient with ID {ingredientId} does not exist.");
            }

            product.RemoveIngredient(ingredientId);
            await _productRepository.UpdateAsync(product);
        }

        public async Task UpdateIngredientQuantityAsync(int productId, int ingredientId, int newQuantity)
        {
            var product = await GetProductAndValidateExistenceAsync(productId, true);

            var ingredient = await _ingredientRepository.GetByIdAsync(ingredientId);
            
            if (ingredient == null)
            {
                throw new KeyNotFoundException($"Ingredient with ID {ingredientId} does not exist.");
            }

            product.UpdateIngredientQuantity(ingredientId, newQuantity);
            await _productRepository.UpdateAsync(product);
        }

        private async Task<Product> GetProductAndValidateExistenceAsync(int productId, bool withDetails = false)
        {
            Product? product;

            if (withDetails)
            {
                product = await _productRepository.GetByIdWithDetailsAsync(productId);
            }
            else
            {
                product = await _productRepository.GetByIdAsync(productId);
            }

            if (product == null)
            {
                throw new KeyNotFoundException($"Product with ID {productId} does not exist.");
            }

            return product;
        }
    }
}
