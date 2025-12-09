using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Application.Common.Validation;
using Application.Features.StaffManagement.Dtos.Commands;

namespace Application.Features.StaffManagement.Dtos.Validators
{
    public class ChangeStaffMemberRoleDtoValidator : AbstractValidator<ChangeStaffMemberRoleDto>
    {
        public ChangeStaffMemberRoleDtoValidator(RoleManager<IdentityRole> roleManager) {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("A valid email is required.");

            RuleFor(x => x.NewRole)
                .NotEmpty().WithMessage("New role name is required.")
                .RoleMustExist(roleManager);
        }
    }
}
