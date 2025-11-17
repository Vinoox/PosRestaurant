using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Ingredients.Dtos.Command;
using FluentValidation;

namespace Application.Features.Ingredients.Dtos.Validatiors
{
    public class UpdateIngredientValidator : AbstractValidator<UpdateIngredientDto>
    {
        public UpdateIngredientValidator()
        {
            When(dto => !string.IsNullOrEmpty(dto.Name), () =>
            {
                RuleFor(dto => dto.Name)
                    .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");
            });

            When(dto => dto.Unit.HasValue, () =>
            {
                RuleFor(dto => dto.Unit)
                    .IsInEnum().WithMessage("Unit must be a valid enum value.");
            });
        }
    }
}
