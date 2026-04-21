using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Validators;
using Identity.Application.Auth.Commands;

namespace Identity.Application.Auth.Validators
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
