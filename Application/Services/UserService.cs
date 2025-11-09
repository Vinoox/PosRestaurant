using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        private readonly IRestaurantService _restaurantService;
        private readonly IStaffAssignmentRepository _staffAssignmentRepository;

        public UserService(UserManager<User> userManager, IPinHasher pinHasher, IJwtTokenGenerator jwtTokenGenerator, IMapper mapper, IRestaurantService restaurantService, IStaffAssignmentRepository staffAssignmentRepository)
        {
            _userManager = userManager;
            _pinHasher = pinHasher;
            _jwtTokenGenerator = jwtTokenGenerator;
            _mapper = mapper;
            _restaurantService = restaurantService;
            _staffAssignmentRepository = staffAssignmentRepository;
        }

        public async Task<AuthenticationResultDto?> AuthenticateAsync(AuthenticateDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
            {
                return null;
            }

            var globalRoles = await _userManager.GetRolesAsync(user);

            var authToken = _jwtTokenGenerator.GenerateAuthenticationToken(user, globalRoles);

            var availableRestaurants = await _restaurantService.GetByUserIdAsync(user.Id);

            return new AuthenticationResultDto
            {
                UserId = user.Id,
                AuthenticationToken = authToken,
                AvailableRestaurants = availableRestaurants
            };
        }

        public async Task<string> GenerateContextualTokenAsync(string userId, int restaurantId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new UnauthorizedAccessException("Użytkownik o podanym ID nie istnieje.");
            }

            var assignment = await _staffAssignmentRepository.GetByUserIdAndRestaurantIdAsync(userId, restaurantId);

            if (assignment == null)
            {
                throw new UnauthorizedAccessException("Użytkownik nie jest przypisany do tej restauracji.");
            }

            var restaurantRoles = new List<string> { assignment.Role.Name };

            return _jwtTokenGenerator.GenerateContextualToken(user, restaurantId, restaurantRoles);
        }

        public async Task<IdentityResult> RegisterAsync(RegisterUserDto dto)
        {
            var user = User.Create(dto.FirstName, dto.LastName, dto.Email);
            var pinHash = _pinHasher.Hash(dto.Pin);
            user.SetPinHash(pinHash);

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded) return result;

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

        public async Task<UserDto?> GetUserByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            return _mapper.Map<UserDto>(user);
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = _userManager.Users.ToList();

            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<IdentityResult> ChangePasswordAsync(string userId, ChangePasswordDto dto)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) throw new KeyNotFoundException("Użytkownik nie znaleziony.");
            return await _userManager.ChangePasswordAsync(user, dto.OldPassword, dto.NewPassword);
        }

        public async Task UpdateProfileAsync(string userId, UpdateUserProfileDto dto)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) throw new KeyNotFoundException("Użytkownik nie znaleziony.");

            user.UpdateProfile(dto.FirstName, dto.LastName);

            await _userManager.UpdateAsync(user);
        }

        public async Task ChangePinAsync(string userId, ChangePinDto dto)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) throw new KeyNotFoundException("Użytkownik nie znaleziony.");

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
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }
        }
    }
}