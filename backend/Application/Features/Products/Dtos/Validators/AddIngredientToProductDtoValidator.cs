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
                .WithMessage("Niepoprawne ID składnika");

            RuleFor(x => x.Amount)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Ilość nie może być ujemna");

            RuleFor(x => x.Unit)
                .IsInEnum()
                .WithMessage("Niepoprawna jednostka");
        }
    }
}
