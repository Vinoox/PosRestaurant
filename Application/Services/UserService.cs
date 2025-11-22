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
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IPinHasher _pinHasher;
        private readonly IMapper _mapper;

        private readonly IValidator<ChangePasswordDto> _changePasswordValidator;
        private readonly IValidator<ChangePinDto> _changePinValidator;
        private readonly IValidator<UpdateUserProfileDto> _updateUserProfileValidator;


        public UserService(
            UserManager<User> userManager,
            IPinHasher pinHasher,
            IJwtTokenGenerator jwtTokenGenerator,
            IMapper mapper,
            IValidator<ChangePasswordDto> changePasswordValidator,
            IValidator<ChangePinDto> changePinValidator,
            IValidator<UpdateUserProfileDto> updateUserProfileValidator)
        {
            _userManager = userManager;
            _pinHasher = pinHasher;
            _mapper = mapper;
            _changePasswordValidator = changePasswordValidator;
            _changePinValidator = changePinValidator;
            _updateUserProfileValidator = updateUserProfileValidator;
        }

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
            await _changePasswordValidator.ValidateAndThrowAsync(dto);
            var user = await FindByIdOrThrowAsync(userId);
            return await _userManager.ChangePasswordAsync(user, dto.OldPassword, dto.NewPassword);
        }

        public async Task UpdateProfileAsync(string userId, UpdateUserProfileDto dto)
        {
            await _updateUserProfileValidator.ValidateAndThrowAsync(dto);
            var user = await FindByIdOrThrowAsync(userId);

            if(dto.FirstName != null) user.UpdateFirstName(dto.FirstName);

            if(dto.LastName != null) user.UpdateLastName(dto.LastName);

            await _userManager.UpdateAsync(user);
        }

        public async Task ChangePinAsync(string userId, ChangePinDto dto)
        {
            await _changePinValidator.ValidateAndThrowAsync(dto);
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