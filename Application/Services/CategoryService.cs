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

namespace Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductRepository _productRepository;
        private readonly IRestaurantService _restaurantService;

        public CategoryService(
            ICategoryRepository categoryRepository, 
            IMapper mapper,
            IRestaurantRepository restaurantRepository,
            IUnitOfWork unitOfWork,
            IProductRepository productRepository,
            IRestaurantService restaurantService
            )
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _restaurantRepository = restaurantRepository;
            _unitOfWork = unitOfWork;
            _productRepository = productRepository;
            _restaurantService = restaurantService;
        }

        public async Task<IEnumerable<CategorySummaryDto>> GetAllByRestaurantIdAsync(int restaurantId)
        {
            var restaurant = await _restaurantService.FindByIdOrThrowAsync(restaurantId);

            var categories = await _categoryRepository.GetAllByRestaurantIdAsync(restaurantId);
            return _mapper.Map<IEnumerable<CategorySummaryDto>>(categories);
        }

        public async Task<CategoryDto?> GetByIdAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            return category != null ? _mapper.Map<CategoryDto>(category) : null;
        }

        public async Task<Category> FindByIdOrThrowAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
                throw new NotFoundException("Category", id);

            return category;
        }
        public async Task AddAsync(CreateCategoryDto dto, int restaurantId)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var restaurant = await _restaurantService.FindByIdOrThrowAsync(restaurantId);

                var existingCategory = await _categoryRepository.GetByNameAsync(dto.Name, restaurantId);
                if (existingCategory != null)
                    throw new BadRequestException($"Category with name '{dto.Name}' already exists in this restaurant.");

                var newCategory = Category.Create(dto.Name, restaurantId);

                _categoryRepository.Add(newCategory);
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task UpdateAsync(UpdateCategoryDto dto, int resturantId)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var restaurant = await _restaurantService.FindByIdOrThrowAsync(resturantId);

                var categoryToUpdate = await _categoryRepository.GetByNameAsync(dto.OldName, resturantId);
                if (categoryToUpdate == null)
                    throw new NotFoundException($"Category with ID {dto.OldName} not found.");

                categoryToUpdate.Name = dto.NewName;

               _categoryRepository.Update(categoryToUpdate);
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task DeleteAsync(int id)
        {
            //await _categoryRepository.DeleteAsync(id);
            throw new NotImplementedException();
        }
    }
}
