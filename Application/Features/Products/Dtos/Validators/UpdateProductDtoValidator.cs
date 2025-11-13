using Application.Features.Products.Dtos.Commands;
using FluentValidation;

namespace Application.Features.Products.Validators
{
    public class UpdateProductDtoValidator : AbstractValidator<UpdateProductDto>
    {
        public UpdateProductDtoValidator()
        {
            When(dto => dto.Name != null, () =>
            {
                RuleFor(dto => dto.Name)
                    .NotEmpty().WithMessage("Nazwa produktu nie może być pusta.")
                    .MaximumLength(100).WithMessage("Nazwa produktu nie może być dłuższa niż 100 znaków.");
            });

            When(dto => dto.Description != null, () =>
            {
                RuleFor(dto => dto.Description)
                    .MaximumLength(500).WithMessage("Opis nie może być dłuższy niż 500 znaków.");
            });

            When(dto => dto.Price.HasValue, () =>
            {
                RuleFor(dto => dto.Price)
                    .GreaterThan(0).WithMessage("Cena musi być większa od zera.");
            });

            When(dto => dto.CategoryId.HasValue, () =>
            {
                RuleFor(dto => dto.CategoryId)
                    .GreaterThan(0).WithMessage("ID kategorii musi być poprawną liczbą.");
            });
        }
    }
}