using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Users.Dtos.Commands;
using FluentValidation;

namespace Application.Features.Users.Dtos.Validators
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
