using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Ingredients.Dtos;
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

        public IngredientService(IIngredientRepository ingredientRepository, IMapper mapper)
        {
            _ingredientRepository = ingredientRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<IngredientDto>> GetAllAsync()
        {
            var ingredients = await _ingredientRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<IngredientDto>>(ingredients);
        }

        public async Task<IngredientDto?> GetByNameAsync(string name)
        {
            var ingredient = await _ingredientRepository.GetByNameAsync(name);
            return ingredient != null ? _mapper.Map<IngredientDto?>(ingredient) : null;
        }

        public async Task<IngredientDto> CreateAsync(CreateIngredientDto newIngredient)
        {
            var ingredient = _mapper.Map<Ingredient>(newIngredient);
            var createdIngredient = await _ingredientRepository.AddAsync(ingredient);
            return _mapper.Map<IngredientDto>(createdIngredient);
        }

        public async Task UpdateAsync(int id, UpdateIngredientDto updatedIngredient)
        {
            var IngredientToUpdate = await _ingredientRepository.GetByIdAsync(id);

            if (IngredientToUpdate == null)
            {
                throw new KeyNotFoundException($"Ingredient with ID {id} not found.");
            }

            _mapper.Map(updatedIngredient, IngredientToUpdate);
            await _ingredientRepository.UpdateAsync(IngredientToUpdate);
        }

        public async Task DeleteAsync(int id)
        {
            await _ingredientRepository.DeleteAsync(id);
        }
    }
}
