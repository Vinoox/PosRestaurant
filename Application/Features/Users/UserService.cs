using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Features.Users.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Features.Users
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<UserDto?> GetByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return user != null ? _mapper.Map<UserDto>(user) : null;
        }

        public async Task<UserDto> RegisterAsync(RegisterUserDto newUser)
        {
            var user = _mapper.Map<User>(newUser);
            var createdUser = await _userRepository.AddAsync(user);
            return _mapper.Map<UserDto>(createdUser);
        }

        public async Task UpdateAsync(int id, UpdateUserDto updatedUser)
        {
            var userToUpdate = await _userRepository.GetByIdAsync(id);
            
            if (userToUpdate == null)
            {
                throw new KeyNotFoundException($"User with ID {id} not found.");
            }

            if(!string.IsNullOrEmpty(updatedUser.NewPassword))
            {
                bool isOldPasswordCorrect = BCrypt.Net.BCrypt.Verify(updatedUser.OldPassword, userToUpdate.PasswordHash);

                if (!isOldPasswordCorrect)
                {
                    throw new ValidationException("Old password is incorrect.");
                }
            }

            _mapper.Map(updatedUser, userToUpdate);

            await _userRepository.UpdateAsync(userToUpdate);
        }

        public async Task DeleteAsync(int id)
        {
            await _userRepository.DeleteAsync(id);
        }
    }
}
