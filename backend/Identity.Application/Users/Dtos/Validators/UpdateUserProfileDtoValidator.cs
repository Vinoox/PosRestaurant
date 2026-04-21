using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Identity.Application.Users.Dtos.Commands;

namespace Identity.Application.Users.Dtos.Validators
{
    public class UpdateUserProfileDtoValidator: AbstractValidator<UpdateUserProfileDto>
    {
        public UpdateUserProfileDtoValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("Imię nie może być puste.")
                .When(x => x.FirstName != null);

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Nazwisko nie może być puste.")
                .When(x => x.LastName != null);
        }
    }
}
