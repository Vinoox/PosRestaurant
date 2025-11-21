using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Features.Restaurants.Dtos.Queries;
using Application.Features.Users.Dtos.Commands;
using Application.Features.Users.Dtos.Queries;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IPinHasher _pinHasher;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IMapper _mapper;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IStaffAssignmentRepository _staffAssignmentRepository;

        public UserService(
            UserManager<User> userManager,
            IPinHasher pinHasher,
            IJwtTokenGenerator jwtTokenGenerator,
            IMapper mapper,
            IRestaurantRepository restaurantRepository,
            IStaffAssignmentRepository staffAssignmentRepository)
        {
            _userManager = userManager;
            _pinHasher = pinHasher;
            _jwtTokenGenerator = jwtTokenGenerator;
            _mapper = mapper;
            _restaurantRepository = restaurantRepository;
            _staffAssignmentRepository = staffAssignmentRepository;
        }

        public async Task<AuthenticationResultDto?> AuthenticateAsync(AuthenticateDto dto)
        {
            var user = await FindByEmailOrThrowAsync(dto.Email);

            var globalRoles = await _userManager.GetRolesAsync(user);

            var authToken = _jwtTokenGenerator.GenerateAuthenticationToken(user, globalRoles);

            var avaailableRestaurantsEntities = await _restaurantRepository.FindByUserIdAsync(user.Id);
            var availableRestaurants = _mapper.Map<IEnumerable<RestaurantSummaryDto>>(avaailableRestaurantsEntities);

            return new AuthenticationResultDto
            {
                UserId = user.Id,
                AuthenticationToken = authToken,
                AvailableRestaurants = availableRestaurants
            };
        }

        public async Task<string> GenerateContextualTokenAsync(string userId, int restaurantId)
        {
            var user = await FindByIdOrThrowAsync(userId);

            var assignment = await _staffAssignmentRepository.FindByUserIdAndRestaurantIdAsync(userId, restaurantId)
                ??throw new UnauthorizedAccessException("Użytkownik nie jest przypisany do tej restauracji.");

            var restaurantRoles = new List<string> { assignment.Role.Name! };

            return _jwtTokenGenerator.GenerateContextualToken(user, restaurantId, restaurantRoles);
        }

        public async Task<IdentityResult> RegisterAsync(RegisterUserDto dto)
        {
            var user = User.Create(dto.FirstName, dto.LastName, dto.Email);
            var pinHash = _pinHasher.Hash(dto.Pin);
            user.SetPinHash(pinHash);

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                if (result.Errors.Any(e => e.Code == "DuplicateEmail"))
                    throw new BadRequestException("Użytkownik o podanym adresie email już istnieje");
                return result;
            }

            await _userManager.AddToRoleAsync(user, "Default");
            return result;
        }

        //public async Task<string> LoginByPinAsync(LoginByPinDto dto)
        //{
        //    var user = await _userManager.FindByEmailAsync(dto.Email);

        //    if (user == null || !_pinHasher.Verify(user.PinHash, dto.Pin))
        //    {
        //        throw new UnauthorizedAccessException("Nieprawidłowy email lub PIN.");
        //    }

        //    var globalRoles = await _userManager.GetRolesAsync(user);

        //    return _jwtTokenGenerator.GenerateAuthenticationToken(user, globalRoles);
        //}

        public async Task<UserDto?> GetByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            return _mapper.Map<UserDto>(user);
        }

        public IEnumerable<UserDto> GetAllUsers()
        {
            var users = _userManager.Users.ToList();

            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<User> FindByEmailOrThrowAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email)
                ??throw new NotFoundException("User", email);

            return user;
        }

        public async Task<User> FindByIdOrThrowAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId)
                ??throw new NotFoundException("User", userId);

            return user;
        }

        public async Task<IdentityResult> ChangePasswordAsync(string userId, ChangePasswordDto dto)
        {
            var user = await FindByIdOrThrowAsync(userId);
            return await _userManager.ChangePasswordAsync(user, dto.OldPassword, dto.NewPassword);
        }

        public async Task UpdateProfileAsync(string userId, UpdateUserProfileDto dto)
        {
            var user = await FindByIdOrThrowAsync(userId);

            if(dto.FirstName != null) user.UpdateFirstName(dto.FirstName);

            if(dto.LastName != null) user.UpdateLastName(dto.LastName);

            await _userManager.UpdateAsync(user);
        }

        public async Task ChangePinAsync(string userId, ChangePinDto dto)
        {
            var user = await FindByIdOrThrowAsync(userId);

            if (!_pinHasher.Verify(user.PinHash, dto.OldPin))
            {
                throw new UnauthorizedAccessException("Stary PIN jest nieprawidłowy.");
            }

            var newPinHash = _pinHasher.Hash(dto.NewPin);
            user.SetPinHash(newPinHash);
            await _userManager.UpdateAsync(user);
        }

        public async Task DeleteAsync(string userId)
        {
            var user = await FindByIdOrThrowAsync(userId);
            await _userManager.DeleteAsync(user);
        }
    }
}