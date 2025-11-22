using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Features.Categories.Dtos.Commands;
using Application.Features.Categories.Dtos.Queries;
using Application.Features.Products.Dtos.Commands;
using Application.Features.Products.Dtos.Queries;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using FluentValidation;

namespace Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRestaurantService _restaurantService;

        private readonly IValidator<CreateCategoryDto> _createCategoryValidator;
        private readonly IValidator<UpdateCategoryDto> _updateCategoryValidator;

        public CategoryService(
            ICategoryRepository categoryRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IRestaurantService restaurantService,
            IValidator<UpdateCategoryDto> updateCategoryValidator,
            IValidator<CreateCategoryDto> createCategoryValidator)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _restaurantService = restaurantService;
            _updateCategoryValidator = updateCategoryValidator;
            _createCategoryValidator = createCategoryValidator;
        }

        public async Task<IEnumerable<CategorySummaryDto>> GetAllByRestaurantIdAsync(int restaurantId)
        {
            var categories = await _categoryRepository.GetAllByRestaurantIdAsync(restaurantId);
            return _mapper.Map<IEnumerable<CategorySummaryDto>>(categories);
        }

        public async Task<CategoryDto?> GetByIdAsync(int restaurantId, int id)
        {
            var category = await _categoryRepository.GetByIdAsync(restaurantId, id);
            return _mapper.Map<CategoryDto>(category);
        }
        public async Task<int> CreateAsync(int restaurantId, CreateCategoryDto dto)
        {
            await _createCategoryValidator.ValidateAndThrowAsync(dto);

            await _restaurantService.FindByIdOrThrowAsync(restaurantId);
            var existingCategory = await _categoryRepository.GetByNameAsync(restaurantId, dto.Name);
            if (existingCategory != null)
                throw new BadRequestException($"Category with name '{dto.Name}' already exists in this restaurant.");

            var newCategory = Category.Create(dto.Name, restaurantId);

            _categoryRepository.Add(newCategory);
            await _unitOfWork.CommitTransactionAsync();
            return newCategory.Id;
        }

        public async Task UpdateAsync(int resturantId, int id, UpdateCategoryDto dto)
        {
            await _updateCategoryValidator.ValidateAndThrowAsync(dto);

            var categoryToUpdate = await FindByIdOrThrowAsync(resturantId, id);
            categoryToUpdate.UpdateName(dto.Name);
            await _unitOfWork.CommitTransactionAsync();
        }

        public async Task DeleteAsync(int restaurantId, int id)
        {
            var categoryToRemove = await FindByIdOrThrowAsync(restaurantId, id);
            _categoryRepository.Remove(categoryToRemove);
            await _unitOfWork.CommitTransactionAsync();
        }

        public async Task<Category> FindByIdOrThrowAsync(int restaurantId, int id)
        {
            var category = await _categoryRepository.GetByIdAsync(restaurantId, id)
                ??throw new NotFoundException("Category", id);

            return category;
        }
    }
}
