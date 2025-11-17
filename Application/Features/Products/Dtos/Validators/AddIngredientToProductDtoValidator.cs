using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Products.Dtos.Commands;
using FluentValidation;

namespace Application.Features.Products.Dtos.Validators
{
    public class AddIngredientToProductDtoValidator : AbstractValidator<AddIngredientToProductDto>
    {
        public AddIngredientToProductDtoValidator() 
        {
            RuleFor(x => x.IngredientId)
                .GreaterThan(0)
                .WithMessage("Id składnika musi być dodatnie.");

            RuleFor(x => x.Amount)
                .GreaterThan(0)
                .WithMessage("Ilość musi być dodatnia");

            RuleFor(x => x.Unit)
                .IsInEnum()
                .WithMessage("Podano nieprawidłową jednostkę");
        }
    }
}
