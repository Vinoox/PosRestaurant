using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Restaurants.Dtos.Commands;
using FluentValidation;

namespace Application.Features.Restaurants.Dtos.Validators
{
    public class CreateRestaurantDtoValidator : AbstractValidator<CreateRestaurantDto>
    {
        public CreateRestaurantDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Restaurant name is required.")
                .MaximumLength(100).WithMessage("Restaurant name must not exceed 100 characters.");
        }
    }
}
