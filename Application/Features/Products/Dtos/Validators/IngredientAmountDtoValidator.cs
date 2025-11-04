using Application.Features.Products.Dtos;
using FluentValidation;

namespace Application.Features.Products.Validators
{
    public class IngredientAmountDtoValidator : AbstractValidator<IngredientAmountDto>
    {
        public IngredientAmountDtoValidator()
        {
            RuleFor(i => i.IngredientId)
                .NotEmpty().WithMessage("ID składnika jest wymagane.")
                .GreaterThan(0).WithMessage("ID składnika musi być poprawną liczbą.");

            RuleFor(i => i.Quantity)
                .NotEmpty().WithMessage("Ilość składnika jest wymagana.")
                .GreaterThan(0).WithMessage("Ilość musi być większa od zera.");
        }
    }
}