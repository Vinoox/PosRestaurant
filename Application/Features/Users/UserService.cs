using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Features.Users;
using Application.Features.Users.Dtos;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Users
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IPinHasher _pinHasher;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IMapper _mapper;

        public UserService(
            UserManager<User> userManager,
            IPinHasher pinHasher,
            IJwtTokenGenerator jwtTokenGenerator,
            IMapper mapper)
        {
            _userManager = userManager;
            _pinHasher = pinHasher;
            _jwtTokenGenerator = jwtTokenGenerator;
            _mapper = mapper;
        }

        public async Task<IdentityResult> RegisterAsync(RegisterUserDto dto)
        {
            var user = User.Create(dto.FirstName, dto.LastName, dto.Email, dto.Duty.Value);
            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                return result;
            }

            var pinHash = _pinHasher.Hash(dto.Pin);
            user.SetPinHash(pinHash);
            return await _userManager.UpdateAsync(user);
        }

        public async Task<string> LoginAsync(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
            {
                throw new UnauthorizedAccessException("Nieprawidłowy email lub hasło.");
            }

            var roles = await _userManager.GetRolesAsync(user);
            return _jwtTokenGenerator.GenerateToken(user, roles);
        }

        public async Task<string> LoginByPinAsync(LoginByPinDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user == null || !_pinHasher.Verify(user.PinHash, dto.Pin))
            {
                throw new UnauthorizedAccessException("Nieprawidłowy email lub PIN.");
            }

            var roles = await _userManager.GetRolesAsync(user);
            return _jwtTokenGenerator.GenerateToken(user, roles);
        }

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

            user.UpdateProfile(dto.FirstName, dto.LastName, dto.Duty);

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