using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Identity.Application.Interfaces;
using Identity.Application.Users.Commands.ChangePassword;
using Identity.Application.Users.Commands.ChangePin;
using Identity.Application.Users.Commands.UpdateUserProfile;
using Identity.Application.Users.Dtos.Queries.GetUser;

// Prawidłowa ścieżka do encji User
using Identity.Domain.Entities;
using Microsoft.AspNetCore.Identity;
// Korzystamy z wyjątków ze współdzielonej biblioteki
using PosRestaurant.Shared.Exceptions;

namespace Identity.Application.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IPinHasher _pinHasher;
        private readonly IMapper _mapper;

        private readonly IValidator<ChangePasswordCommand> _changePasswordValidator;
        private readonly IValidator<ChangePinCommand> _changePinValidator;
        private readonly IValidator<UpdateUserProfileCommand> _updateUserProfileValidator;

        public UserService(
            UserManager<User> userManager,
            IPinHasher pinHasher,
            IMapper mapper,
            IValidator<ChangePasswordCommand> changePasswordValidator,
            IValidator<ChangePinCommand> changePinValidator,
            IValidator<UpdateUserProfileCommand> updateUserProfileValidator)
        {
            _userManager = userManager;
            _pinHasher = pinHasher;
            _mapper = mapper;
            _changePasswordValidator = changePasswordValidator;
            _changePinValidator = changePinValidator;
            _updateUserProfileValidator = updateUserProfileValidator;
        }

        public async Task<GetUserQuery?> GetByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return _mapper.Map<GetUserQuery>(user);
        }

        public IEnumerable<GetUserQuery> GetAllUsers()
        {
            var users = _userManager.Users.ToList();
            return _mapper.Map<IEnumerable<GetUserQuery>>(users);
        }

        public async Task<User> FindByEmailOrThrowAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email)
                ?? throw new NotFoundException("User", email);

            return user;
        }

        public async Task<User> FindByIdOrThrowAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId)
                ?? throw new NotFoundException("User", userId);

            return user;
        }

        public async Task<IdentityResult> ChangePasswordAsync(string userId, ChangePasswordCommand dto)
        {
            await _changePasswordValidator.ValidateAndThrowAsync(dto);
            var user = await FindByIdOrThrowAsync(userId);
            return await _userManager.ChangePasswordAsync(user, dto.OldPassword, dto.NewPassword);
        }

        public async Task UpdateProfileAsync(string userId, UpdateUserProfileCommand dto)
        {
            await _updateUserProfileValidator.ValidateAndThrowAsync(dto);
            var user = await FindByIdOrThrowAsync(userId);

            if (dto.FirstName != null) user.UpdateFirstName(dto.FirstName);

            if (dto.LastName != null) user.UpdateLastName(dto.LastName);

            await _userManager.UpdateAsync(user);
        }

        public async Task ChangePinAsync(string userId, ChangePinCommand dto)
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