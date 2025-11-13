using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Products.Dtos.Commands;
using Application.Features.Products.Validators;
using FluentValidation;

namespace Application.Features.Products.Dtos.Validators
{
    public class CreateProductDtoValidator : AbstractValidator<CreateProductDto>
    {
        public CreateProductDtoValidator()
        {
            RuleFor(dto => dto.Name)
                .NotEmpty().WithMessage("Nazwa produktu jest wymagana.")
                .MaximumLength(100).WithMessage("Nazwa produktu nie może być dłuższa niż 100 znaków.");

            RuleFor(dto => dto.Description)
            .MaximumLength(500).WithMessage("Opis nie może być dłuższy niż 500 znaków.");

            RuleFor(dto => dto.Price)
                .NotEmpty().WithMessage("Cena produktu jest wymagana.")
                .GreaterThan(0).WithMessage("Cena musi być większa od zera.");

            RuleFor(dto => dto.CategoryId)
                .NotEmpty().WithMessage("ID kategorii jest wymagane.")
                .GreaterThan(0).WithMessage("ID kategorii musi być poprawną liczbą.");

        }
    }
}
