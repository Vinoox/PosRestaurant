using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Features.Products.Dtos;
using Application.Features.Products.Dtos.Commands;
using Application.Features.Products.Dtos.Queries;
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
        private readonly IIngredientRepository _ingredientRepository;
        private readonly IRestaurantService _restaurantService;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(
            IProductRepository productRepository,
            IIngredientRepository ingredientRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IRestaurantService restaurantService,
            ICategoryService categoryService)
        {
            _productRepository = productRepository;
            _ingredientRepository = ingredientRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _restaurantService = restaurantService;
            _categoryService = categoryService;
        }


        public async Task<IEnumerable<ProductDto>> GetAllAsync(int restaurantId)
        {
            var restaurant = await _restaurantService.FindByIdOrThrowAsync(restaurantId);

            var products = await _productRepository.GetAllAsync(restaurantId);
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<IEnumerable<ProductDto>> GetAllByCategoryIdAsync(int restaurantId, int categoryId)
        {
            var restaurant = await _restaurantService.FindByIdOrThrowAsync(restaurantId);
            var category = await _categoryService.FindByIdOrThrowAsync(categoryId);

            if (restaurant.Id != category.RestaurantId)
                throw new BadRequestException("");

            var products = await _productRepository.GetAllByCategoryIdAsync(restaurantId, categoryId);
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<ProductDto> GetByIdAsync(int restaurantId, int productId)
        {
            await _restaurantService.FindByIdOrThrowAsync(restaurantId);

            var product = await _productRepository.GetByProductIdAsync(productId);

            return _mapper.Map<ProductDto>(product);
        }

        public async Task<int> CreateAsync(int restaurantId, CreateProductDto dto)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var restaurant = await _restaurantService.FindByIdOrThrowAsync(restaurantId);

                var category = await _categoryService.FindByIdOrThrowAsync(dto.CategoryId);

                if (category.RestaurantId != restaurantId)
                    throw new BadRequestException("The specified category does not belong to the given restaurant.");

                var existingProduct = await _productRepository.GetByProductNameAsync(restaurantId, dto.Name);
                if (existingProduct != null)
                    throw new BadRequestException($"Product with name '{dto.Name}' already exists in this restaurant.");

                var product = Product.Create(dto.Name, dto.Description, dto.Price, dto.CategoryId, restaurantId);

                _productRepository.Add(product);
                await _unitOfWork.CommitTransactionAsync();
                return product.Id;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task DeleteAsync(int productId)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var product = await _productRepository.GetByProductIdAsync(productId);
                if (product == null)
                    throw new BadRequestException("Product doesnt exists");

                _productRepository.Delete(product);
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }



            //public async Task UpdateAsync(int id, UpdateProductDto updatedProduct)
            //{
            //    var product = await GetProductAndValidateExistenceAsync(id, true);

            //    if (updatedProduct.CategoryId.HasValue)
            //    {
            //        var category = await _categoryRepository.GetByIdAsync(updatedProduct.CategoryId.Value);
            //        if (category == null)
            //        {
            //            throw new KeyNotFoundException($"Category with ID {updatedProduct.CategoryId} does not exist.");
            //        }
            //    }

            //    var updateParams = _mapper.Map<ProductUpdateParams>(updatedProduct);

            //    product.Update(updateParams);

            //    await _productRepository.UpdateAsync(product);
            //}

            //public async Task AddIngredientToProductAsync(int productId, IngredientAmountDto ingredientAmountDto)
            //{
            //    var product = await GetProductAndValidateExistenceAsync(productId, true);

            //    var ingredientExists = await _ingredientRepository.GetByIdAsync(ingredientAmountDto.IngredientId);

            //    if (ingredientExists == null)
            //    {
            //        throw new KeyNotFoundException($"Ingredient with ID {ingredientAmountDto.IngredientId} does not exist.");
            //    }

            //    product.AddIngredient(ingredientAmountDto.IngredientId, ingredientAmountDto.Quantity);
            //    await _productRepository.UpdateAsync(product);
            //}

            //public async Task RemoveIngredientFromProductAsync(int productId, int ingredientId)
            //{
            //    var product = await GetProductAndValidateExistenceAsync(productId, true);

            //    var ingredient = await _ingredientRepository.GetByIdAsync(ingredientId);

            //    if (ingredient == null)
            //    {
            //        throw new KeyNotFoundException($"Ingredient with ID {ingredientId} does not exist.");
            //    }

            //    product.RemoveIngredient(ingredientId);
            //    await _productRepository.UpdateAsync(product);
            //}

            //public async Task UpdateIngredientQuantityAsync(int productId, int ingredientId, int newQuantity)
            //{
            //    var product = await GetProductAndValidateExistenceAsync(productId, true);

            //    var ingredient = await _ingredientRepository.GetByIdAsync(ingredientId);

            //    if (ingredient == null)
            //    {
            //        throw new KeyNotFoundException($"Ingredient with ID {ingredientId} does not exist.");
            //    }

            //    product.UpdateIngredientQuantity(ingredientId, newQuantity);
            //    await _productRepository.UpdateAsync(product);
            //}

            //private async Task<Product> GetProductAndValidateExistenceAsync(int productId, bool withDetails = false)
            //{
            //    Product? product;

            //    if (withDetails)
            //    {
            //        product = await _productRepository.GetByIdWithDetailsAsync(productId);
            //    }
            //    else
            //    {
            //        product = await _productRepository.GetByIdAsync(productId);
            //    }

            //    if (product == null)
            //    {
            //        throw new KeyNotFoundException($"Product with ID {productId} does not exist.");
            //    }

            //    return product;
            //}



            //public Task<IEnumerable<SimpleProductDto>> GetByRestaurantIdAndNameOrThrowAsync(int restuarantId, string name)
            //{
            //    throw new NotImplementedException();
            //}

            //public Task<ProductDto?> GetByIdOrThrowAsync(int id)
            //{
            //    throw new NotImplementedException();
            //}

            //public Task<ProductDto> CreateOrThrowAsync(int restaurantId, CreateProductDto dto)
            //{
            //    await _unitOfWork.BeginTransactionAsync();

            //    try
            //    {
            //        var category = await _categoryRepository.GetByIdAsync(dto.CategoryId);
            //        if (category == null)
            //            throw new BadRequestException($"Category with ID {dto.CategoryId} does not exist.");

            //        if (category.RestaurantId != restaurantId)
            //            throw new BadRequestException("The specified category does not belong to the given restaurant.");

            //        var existingProduct = await _productRepository.GetByNameOrThrowAsync(dto.Name, restaurantId);
            //        if (existingProduct != null)
            //            throw new BadRequestException($"Product with name '{dto.Name}' already exists in this restaurant.");

            //        var product = Product.Create(dto.Name, dto.Description, dto.Price, dto.CategoryId, restaurantId);

            //        _productRepository.Add(product);
            //        await _unitOfWork.CommitTransactionAsync();
            //        return _mapper.Map<ProductDto>(product);
            //    }
            //    catch (Exception)
            //    {
            //        await _unitOfWork.RollbackTransactionAsync();
            //        throw;
            //    }
            //}

            //public Task DeleteAsync(int resturantId, DeleteProductDto productToDelete)
            //{
            //    throw new NotImplementedException();
            //}
        }
    }
}
