using FluentValidation;

namespace Catalog.Application.Categories.Commands.CreateCategory
{
    public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
    {
        public CreateCategoryCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Nazwa kategorii jest wymagana.")
                .MaximumLength(100).WithMessage("Nazwa nie może przekroczyć 100 znaków.");

            RuleFor(x => x.RestaurantId)
                .NotEmpty().WithMessage("Brak identyfikatora restauracji.");
        }
    }
}