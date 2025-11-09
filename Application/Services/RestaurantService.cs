using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Restaurants.Dtos;
using Application.Features.Restaurants.Dtos.Commands;
using Application.Features.Restaurants.Dtos.Queries;
using Application.Features.Users.Dtos;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Application.Services
{
    public class RestaurantService : IRestaurantService
    {
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IProductRepository _productRepository;
        private readonly IIngredientRepository _ingredientRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStaffAssignmentRepository _staffAssignmentRepository;

        public RestaurantService(
            IRestaurantRepository restaurantRepository,
            IProductRepository productRepository,
            IIngredientRepository ingredientRepository,
            ICategoryRepository categoryRepository,
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            IMapper mapper,
            IStaffAssignmentRepository staffAssignmentRepository,
            IUnitOfWork unitOfWork)
        {
            _restaurantRepository = restaurantRepository;
            _productRepository = productRepository;
            _ingredientRepository = ingredientRepository;
            _categoryRepository = categoryRepository;
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _staffAssignmentRepository = staffAssignmentRepository;
        }

        public async Task AddStaffMemberAsync(int id, AddStaffMemberDto dto)
        {
            await _unitOfWork.BeginTransactionAsync();
            {
                try
                {
                    var userToAdd = await _userManager.FindByEmailAsync(dto.Email);
                    if (userToAdd == null)
                        throw new Exception($"Użytkownik o emailu '{dto.Email}' nie został znaleziony.");

                    var restaurant = await _restaurantRepository.GetByIdAsync(id);
                    if (restaurant == null)
                        throw new Exception($"Restauracja o ID '{id}' nie została znaleziona.");

                    var role = await _roleManager.FindByNameAsync(dto.RoleName);
                    if (role == null)
                        throw new Exception($"Rola '{dto.RoleName}' nie została znaleziona.");

                    var existingAssignment = await _staffAssignmentRepository.GetByUserIdAndRestaurantIdAsync(userToAdd.Id, restaurant.Id);
                    if (existingAssignment != null)
                        throw new Exception($"Użytkownik '{dto.Email}' jest już przypisany do restauracji '{restaurant.Name}'.");

                    var assignment = StaffAssignment.Create(userToAdd, restaurant, role);
                    await _staffAssignmentRepository.AddAsync(assignment);

                    await _unitOfWork.CommitTransactionAsync();
                }
                catch (Exception)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    throw;
                }
            }
        }

        public async Task<int> CreateAsync(CreateRestaurantDto dto, string creatorUserId)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var creatorUser = await _userManager.FindByIdAsync(creatorUserId);
                if (creatorUser == null)
                {
                    throw new InvalidOperationException($"User with ID {creatorUserId} does not exist.");
                }

                var restaurant = Restaurant.Create(dto.Name);
                await _restaurantRepository.CreateAsync(restaurant);

                var adminRole = await _roleManager.FindByNameAsync("RestaurantAdmin");

                var assignment = StaffAssignment.Create(creatorUser, restaurant, adminRole);

                await _staffAssignmentRepository.AddAsync(assignment);
                await _unitOfWork.CommitTransactionAsync();
                return restaurant.Id;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<RestaurantDto?> GetByIdAsync(int id)
        {
            var restaurant = await _restaurantRepository.GetByIdAsync(id);

            if (restaurant == null)
            {
                return null;
            }

            return _mapper.Map<RestaurantDto>(restaurant);
        }

        public async Task<IEnumerable<RestaurantSummaryDto>> GetByUserIdAsync(string userId)
        {
            var restaurants = await _restaurantRepository.FindByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<RestaurantSummaryDto>>(restaurants);
        }

        
    }
}
