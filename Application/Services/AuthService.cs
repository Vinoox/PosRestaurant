using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Features.Auth.Commands;
using Application.Features.Auth.Queries;
using Application.Features.Restaurants.Dtos.Queries;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly IPinHasher _pinHasher;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IMapper _mapper;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IStaffAssignmentRepository _staffAssignmentRepository;
        private readonly IUserService _userService;

        private readonly IValidator<RegisterUserDto> _registerUserValidator;
        private readonly IValidator<AuthenticateDto> _authenticateValidator;
        private readonly IValidator<LoginByPinDto> _loginByPinValidator;
        private readonly IValidator<SelectRestaurantDto> _selectRestaurantValidator;


        public AuthService(
            UserManager<User> userManager,
            IUserService userService,
            IPinHasher pinHasher,
            IJwtTokenGenerator jwtTokenGenerator,
            IMapper mapper,
            IRestaurantRepository restaurantRepository,
            IStaffAssignmentRepository staffAssignmentRepository,
            IValidator<AuthenticateDto> authenticateValidator,
            IValidator<LoginByPinDto> loginByPinValidator,
            IValidator<SelectRestaurantDto> selectRestaurantValidator,
            IValidator<RegisterUserDto> registerValidator)
        {
            _userManager = userManager;
            _userService = userService;
            _pinHasher = pinHasher;
            _jwtTokenGenerator = jwtTokenGenerator;
            _mapper = mapper;
            _restaurantRepository = restaurantRepository;
            _staffAssignmentRepository = staffAssignmentRepository;
            _authenticateValidator = authenticateValidator;
            _loginByPinValidator = loginByPinValidator;
            _selectRestaurantValidator = selectRestaurantValidator;
            _registerUserValidator = registerValidator;
        }

        public async Task<AuthenticationResultDto?> AuthenticateAsync(AuthenticateDto dto)
        {
            await _authenticateValidator.ValidateAndThrowAsync(dto);
            var user = await _userService.FindByEmailOrThrowAsync(dto.Email);

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

        public async Task<string> GenerateContextualTokenAsync(string userId, SelectRestaurantDto dto)
        {
            await _selectRestaurantValidator.ValidateAndThrowAsync(dto);

            var user = await _userService.FindByIdOrThrowAsync(userId);

            var assignment = await _staffAssignmentRepository.FindByUserIdAndRestaurantIdAsync(userId, dto.RestaurantId)
                ?? throw new UnauthorizedAccessException("Użytkownik nie jest przypisany do tej restauracji.");

            var restaurantRoles = new List<string> { assignment.Role.Name! };

            return _jwtTokenGenerator.GenerateContextualToken(user, dto.RestaurantId, restaurantRoles);
        }

        public async Task<IdentityResult> RegisterAsync(RegisterUserDto dto)
        {
            await _registerUserValidator.ValidateAndThrowAsync(dto);
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
    }
}
