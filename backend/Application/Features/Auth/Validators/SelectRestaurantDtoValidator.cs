using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Auth.Commands;
using FluentValidation;
using FluentValidation.Validators;

namespace Application.Features.Auth.Validators
{
    public class SelectRestaurantDtoValidator : AbstractValidator<SelectRestaurantDto>
    {
        public SelectRestaurantDtoValidator()
        {
            RuleFor(dto => dto.RestaurantId)
                .NotEmpty().WithMessage("Id restauracji jest wymagane.");
        }
    }
}
