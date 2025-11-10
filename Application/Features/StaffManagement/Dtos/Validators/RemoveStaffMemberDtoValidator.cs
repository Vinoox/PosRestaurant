using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.StaffManagement.Dtos.Commands;
using FluentValidation;

namespace Application.Features.StaffManagement.Dtos.Validators
{
    public class RemoveStaffMemberDtoValidator : AbstractValidator<RemoveStaffMemberDto>
    {
        public RemoveStaffMemberDtoValidator() 
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("A valid email address is required.");
        }
    }
}
