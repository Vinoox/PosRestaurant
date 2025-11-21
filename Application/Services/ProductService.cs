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
using Domain.Common;
using Domain.Entities;
using Domain.Interfaces;
using FluentValidation;

namespace Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IIngredientService _ingredientService;

        public ProductService(
            IProductRepository productRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            ICategoryService categoryService,
            IIngredientService ingredientService)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _categoryService = categoryService;
            _ingredientService = ingredientService;
        }


        public async Task<IEnumerable<ProductDto>> GetAllAsync(int restaurantId)
        {
            var products = await _productRepository.GetAllAsync(restaurantId);
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<IEnumerable<ProductDto>> GetAllByCategoryIdAsync(int restaurantId, int categoryId)
        {
            var products = await _productRepository.GetAllByCategoryIdAsync(restaurantId, categoryId);
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<ProductDto> GetByIdAsync(int restaurantId, int productId)
        {
            var product = await _productRepository.GetByIdAsync(restaurantId, productId);

            return _mapper.Map<ProductDto>(product);
        }

        public async Task<int> CreateAsync(int restaurantId, CreateProductDto dto)
        {
            await _categoryService.FindByIdOrThrowAsync(restaurantId, dto.CategoryId);

            var existingProduct = await _productRepository.GetByNameAsync(restaurantId, dto.Name);
            if (existingProduct != null)
                throw new BadRequestException($"Product with name '{dto.Name}' already exists in this restaurant.");

            var product = Product.Create(dto.Name, dto.Description, dto.Price, dto.CategoryId, restaurantId);

            _productRepository.Add(product);
            await _unitOfWork.CommitTransactionAsync();
            return product.Id;
        }

        public async Task UpdateDetailsAsync(int restaurantId, int productId, UpdateProductDto dto)
        {
            var productToUpdate = await FindByIdOrThrowAsync(restaurantId, productId);

            if(dto.Name != null)
            {
                productToUpdate.UpdateName(dto.Name);
            }

            if (dto.Description != null)
            {
                productToUpdate.UpdateDescription(dto.Description);
            }

            if(dto.Price.HasValue)
            {
                productToUpdate.UpdatePrice(dto.Price.Value);
            }

            if (dto.CategoryId.HasValue)
            {
                await _categoryService.FindByIdOrThrowAsync(restaurantId, dto.CategoryId.Value);
                productToUpdate.UpdateCategory(dto.CategoryId.Value);
            }

            await _unitOfWork.CommitTransactionAsync();
        }

        public async Task DeleteAsync(int restaurantId, int productId)
        {
            var product = await FindByIdOrThrowAsync(restaurantId, productId);
            _productRepository.Remove(product);
            await _unitOfWork.CommitTransactionAsync();

        }

        public async Task<Product> FindByIdOrThrowAsync(int restaurantId, int productId)
        {
            var product = await _productRepository.GetByIdAsync(restaurantId, productId)
                ??throw new NotFoundException("Product", productId);

            return product;
        }




        public async Task<ProductIngredientDto> AddIngredientToProductAsync(int restaurantId, int productId, AddIngredientToProductDto dto)
        {
            var product = await FindByIdOrThrowAsync(restaurantId, productId);
            var ingredient = await _ingredientService.FindByIdOrThrowAsync(restaurantId, dto.IngredientId);

            ProductIngredient productIngredient = product.AddIngredient(ingredient, dto.Amount, dto.Unit);

            await _unitOfWork.CommitTransactionAsync();
            return _mapper.Map<ProductIngredientDto>(productIngredient);
        }

        public async Task RemoveIngredientFromProductAsync(int restaurantId, int productId, int ingredientId)
        {
            var product = await _productRepository.GetByIdWithIngredientsAsync(restaurantId, productId)
                ??throw new NotFoundException($"Produkt o ID {productId} nie został znaleziony.");

            product.RemoveIngredient(ingredientId);


            await _unitOfWork.CommitTransactionAsync();
        }

        public async Task UpdateIngredientAmountAsync(int restaurantId, int productId, int ingredientId, decimal newAmount)
        {
            var product = await _productRepository.GetByIdWithIngredientsAsync(restaurantId, productId)
                ??throw new NotFoundException($"Produkt o ID {productId} nie został znaleziony.");

            product.UpdateIngredientAmount(ingredientId, newAmount);


            await _unitOfWork.CommitTransactionAsync();
        }

        public async Task ClearIngredientListAsync(int restaurantId, int productId)
        {
            var product = await _productRepository.GetByIdWithIngredientsAsync(restaurantId, productId)
                ??throw new NotFoundException($"Produkt o ID {productId} nie został znaleziony.");

            product.ClearIngredients();
            await _unitOfWork.CommitTransactionAsync();
        }
    }
}
