using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Exceptions;
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
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStaffAssignmentRepository _staffAssignmentRepository;
        private readonly IUserService _userService;
        private readonly IStaffManagementService _staffManagementService;
        private readonly IRestaurantService restaurantService;

        public RestaurantService(
            IStaffManagementService staffManagementService,
            IRestaurantRepository restaurantRepository,
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            IMapper mapper,
            IStaffAssignmentRepository staffAssignmentRepository,
            IUnitOfWork unitOfWork,
            IUserService userService)
        {
            _staffManagementService = staffManagementService;
            _restaurantRepository = restaurantRepository;
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _staffAssignmentRepository = staffAssignmentRepository;
            _userService = userService;
        }


        public async Task<int> CreateAsync(CreateRestaurantDto dto, string creatorUserId)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var creatorUser = await _userService.FindByIdOrThrowAsync(creatorUserId);
                var restaurant = Restaurant.Create(dto.Name);

                _restaurantRepository.CreateAsync(restaurant);
                await _staffManagementService.AddInitialMemberAsync(restaurant, creatorUser);
                await _unitOfWork.CommitTransactionAsync();
                return restaurant.Id;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<RestaurantDto?> GetByIdAsync(int id)
        {
            var restaurant = await _restaurantRepository.GetByIdAsync(id);


            return _mapper.Map<RestaurantDto>(restaurant);
        }

        public async Task<Restaurant> FindByIdOrThrowAsync(int id)
        {
            var restaurant = await _restaurantRepository.GetByIdAsync(id);
            if (restaurant == null)
                throw new NotFoundException("Restaurant", id);
            
            return restaurant;
        }

        public async Task<IEnumerable<RestaurantSummaryDto>> GetByUserIdAsync(string userId)
        {
            var restaurants = await _restaurantRepository.FindByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<RestaurantSummaryDto>>(restaurants);
        }
    }
}
