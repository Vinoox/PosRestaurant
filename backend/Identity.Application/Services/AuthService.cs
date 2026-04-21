using System;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Identity.Application.Auth.Commands;
using Identity.Application.Auth.Queries;
using Identity.Application.Interfaces;
using Identity.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using PosRestaurant.Shared.Exceptions;

namespace Identity.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly IPinHasher _pinHasher;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IUserService _userService;

        private readonly IValidator<RegisterUserDto> _registerUserValidator;
        private readonly IValidator<AuthenticateDto> _authenticateValidator;
        private readonly IValidator<LoginByPinDto> _loginByPinValidator;

        public AuthService(
            UserManager<User> userManager,
            IUserService userService,
            IPinHasher pinHasher,
            IJwtTokenGenerator jwtTokenGenerator,
            IValidator<AuthenticateDto> authenticateValidator,
            IValidator<LoginByPinDto> loginByPinValidator,
            IValidator<RegisterUserDto> registerValidator)
        {
            _userManager = userManager;
            _userService = userService;
            _pinHasher = pinHasher;
            _jwtTokenGenerator = jwtTokenGenerator;
            _authenticateValidator = authenticateValidator;
            _loginByPinValidator = loginByPinValidator;
            _registerUserValidator = registerValidator;
        }

        public async Task<AuthenticationResultDto?> AuthenticateAsync(AuthenticateDto dto)
        {
            await _authenticateValidator.ValidateAndThrowAsync(dto);

            var user = await _userService.FindByEmailOrThrowAsync(dto.Email);
            var globalRoles = await _userManager.GetRolesAsync(user);
            var authToken = _jwtTokenGenerator.GenerateAuthenticationToken(user, globalRoles);

            // Zwracamy wyłącznie token i ID. Identity nie wie, do jakich restauracji masz dostęp.
            return new AuthenticationResultDto
            {
                UserId = user.Id,
                AuthenticationToken = authToken
            };
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
        //    await _loginByPinValidator.ValidateAndThrowAsync(dto);

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