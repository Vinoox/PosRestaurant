using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Features.StaffManagement.Dtos.Commands;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Application.Services
{
    public class StaffManagementService : IStaffManagementService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IStaffAssignmentRepository _staffAssignmentRepository;
        private readonly RoleManager<IdentityRole> _roleManager;

        public StaffManagementService(
            IUnitOfWork unitOfWork,
            IRestaurantRepository restaurantRepository,
            IUserService userService,
            IStaffAssignmentRepository staffAssignmentRepository,
            RoleManager<IdentityRole> roleManager)
        {
            _unitOfWork = unitOfWork;
            _restaurantRepository = restaurantRepository;
            _userService = userService;
            _staffAssignmentRepository = staffAssignmentRepository;
            _roleManager = roleManager;
        }
        public async Task AddStaffMemberAsync(int restaurantId, AddStaffMemberDto dto)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var userToAdd = await _userService.FindByEmailOrThrowAsync(dto.Email);
                var restaurant = await _restaurantRepository.GetByIdAsync(restaurantId);
                if (restaurant == null)
                    throw new NotFoundException("Restauracja", restaurantId);


                var role = await _roleManager.FindByNameAsync(dto.RoleName);
                if (role == null)
                    throw new BadRequestException($"Rola '{dto.RoleName}' nie została znaleziona.");

                var existingAssignment = await _staffAssignmentRepository.FindByUserIdAndRestaurantIdAsync(userToAdd.Id, restaurant.Id);
                if (existingAssignment != null)
                    throw new BadRequestException($"Użytkownik jest już przypisany do tej restauracji.");

                var assignment = StaffAssignment.Create(userToAdd, restaurant, role);

                _staffAssignmentRepository.Add(assignment);
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task AddInitialMemberAsync(Restaurant restaurant, User user)
        {
            var adminRole = await _roleManager.FindByNameAsync("RestaurantAdmin");
            if (adminRole == null)
                throw new InvalidOperationException("Rola 'RestaurantAdmin' nie została znaleziona.");

            var assignment = StaffAssignment.Create(user, restaurant, adminRole);
            _staffAssignmentRepository.Add(assignment);
        }

        public async Task RemoveStaffMemberAsync(int restaurantId, RemoveStaffMemberDto dto)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var userToRemove = await _userService.FindByEmailOrThrowAsync(dto.Email);
                var restaurant = await _restaurantRepository.GetByIdAsync(restaurantId);

                var existingAssignment = await FindAssignmentOrThrowAsync(userToRemove.Id, restaurant.Id);

                if(existingAssignment.Role.Name == "RestaurantAdmin")
                        throw new BadRequestException("Nie można usunąć administratora restauracji.");

                _staffAssignmentRepository.Remove(existingAssignment);
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task ChangeStaffMemberRoleAsync(int restaurantId, ChangeStaffMemberRoleDto dto)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var userToChange = await _userService.FindByEmailOrThrowAsync(dto.Email);
                var restaurant = await _restaurantRepository.GetByIdAsync(restaurantId);

                var newRole = await _roleManager.FindByNameAsync(dto.NewRole);
                if (newRole == null)
                    throw new BadRequestException($"Rola '{dto.NewRole}' nie została znaleziona.");

                var existingAssignment = await FindAssignmentOrThrowAsync(userToChange.Id, restaurant.Id);

                if(existingAssignment.Role.Name == "RestaurantAdmin")
                {
                    int adminCount = await _restaurantRepository.CountByIdAndRoleNameAsync(restaurant.Id, "RestaurantAdmin");
                    if (adminCount <= 1)
                        throw new BadRequestException("Nie można zmienić roli ostatniego administratora restauracji.");
                }


                _staffAssignmentRepository.Remove(existingAssignment);
                var updatedAssignment = StaffAssignment.Create(userToChange, restaurant, newRole);
                _staffAssignmentRepository.Add(updatedAssignment);
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        private async Task<StaffAssignment> FindAssignmentOrThrowAsync(string userId, int restaurantId)
        {
            var assignment = await _staffAssignmentRepository.FindByUserIdAndRestaurantIdAsync(userId, restaurantId);
            if (assignment == null)
                throw new NotFoundException($"Pracownik o podanym ID nie jest przypisany do tej restauracji.");
            return assignment;
        }
    }
}
