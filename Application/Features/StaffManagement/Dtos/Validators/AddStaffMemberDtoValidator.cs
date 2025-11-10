using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Application.Common.Validation;
using Microsoft.AspNetCore.Identity;
using Application.Features.StaffManagement.Dtos.Commands;


namespace Application.Features.StaffManagement.Dtos.Validators
{
    public class AddStaffMemberDtoValidator : AbstractValidator<AddStaffMemberDto>
    {
        public AddStaffMemberDtoValidator(RoleManager<IdentityRole> roleManager) 
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("A valid email address is required.");

            RuleFor(x => x.RoleName)
                .NotEmpty().WithMessage("Role name is required.")
                .RoleMustExist(roleManager);
        }
    }
}
