using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Features.Ingredients.Dtos.Command;
using Application.Features.Ingredients.Dtos.Queries;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class IngredientService : IIngredientService
    {
        private readonly IIngredientRepository _ingredientRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public IngredientService(
            IIngredientRepository ingredientRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            _ingredientRepository = ingredientRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<IngredientDto>> GetAllAsync(int restaurantId)
        {
            var ingredients = await _ingredientRepository.GetAllAsync(restaurantId);
            return _mapper.Map<IEnumerable<IngredientDto>>(ingredients);
        }

        public async Task<IngredientDto?> GetByIdAsync(int restaurantId, int id)
        {
            var ingredient = await _ingredientRepository.GetByIdAsync(restaurantId, id);

            return _mapper.Map<IngredientDto>(ingredient);
        }

        public async Task<IngredientDto?> GetByNameAsync(int restaurantId, string name)
        {
            var ingredient = await _ingredientRepository.GetByNameAsync(restaurantId, name);
            return _mapper.Map<IngredientDto?>(ingredient);
        }

        public async Task<int> CreateAsync(int restaurantId, CreateIngredientDto dto)
        {
            var existingIngredient = await _ingredientRepository.GetByNameAsync(restaurantId, dto.Name);
            if (existingIngredient != null)
                throw new BadRequestException($"Ingredient with name '{dto.Name}' already exists in this restaurant.");


            var ingredient = Ingredient.Create(dto.Name, dto.Unit, restaurantId);
            _ingredientRepository.Add(ingredient);

            await _unitOfWork.CommitTransactionAsync();
            return ingredient.Id;
        }

        public async Task DeleteAsync(int restaurantId, int id)
        {
            var ingredientToRemove = await FindByIdOrThrowAsync(restaurantId, id);
            _ingredientRepository.Remove(ingredientToRemove);
            await _unitOfWork.CommitTransactionAsync();
        }


        public async Task UpdateAsync(int restaurantId, int id, UpdateIngredientDto dto)
        {
            throw new NotImplementedException();
        }


        public async Task<Ingredient> FindByIdOrThrowAsync(int restaurntId, int id)
        {
            var ingredient = await _ingredientRepository.GetByIdAsync(restaurntId, id);
            if (ingredient == null)
                throw new NotFoundException("Ingredient", id);

            return ingredient;
        }
    }
}
