using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Features.Ingredients.Dtos.Validatiors
{
    public class CreateIngredientDtoValidator : AbstractValidator<CreateIngredientDto>
    { 
        public CreateIngredientDtoValidator()
        {
            RuleFor(dto => dto.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

            RuleFor(dto => dto.Unit)
                .IsInEnum().WithMessage("Unit must be a valid enum value.");
        }
    }
}
