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
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace Application.Services
{
    public class RestaurantService : IRestaurantService
    {
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        private readonly IStaffManagementService _staffManagementService;

        private readonly IValidator<CreateRestaurantDto> _createRestaurantValidator;

        public RestaurantService(
            IStaffManagementService staffManagementService,
            IRestaurantRepository restaurantRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IUserService userService,
            IValidator<CreateRestaurantDto> createRestaurantValidator)
        {
            _staffManagementService = staffManagementService;
            _restaurantRepository = restaurantRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userService = userService;
            _createRestaurantValidator = createRestaurantValidator;
        }


        public async Task<int> CreateAsync(CreateRestaurantDto dto, string creatorUserId)
        {
            await _createRestaurantValidator.ValidateAndThrowAsync(dto);
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var creatorUser = await _userService.FindByIdOrThrowAsync(creatorUserId);
                var restaurant = Restaurant.Create(dto.Name);

                _restaurantRepository.Add(restaurant);
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
