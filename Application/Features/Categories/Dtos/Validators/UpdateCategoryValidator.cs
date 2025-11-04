using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Features.Categories.Dtos.Validators
{
    public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryDto>
    {
        public UpdateCategoryValidator()
        {
            When(dto => !string.IsNullOrEmpty(dto.Name), () =>
            {
                RuleFor(dto => dto.Name)
                    .NotEmpty().WithMessage("Nazwa kategorii jest wymagana.")
                    .MaximumLength(100).WithMessage("Nazwa kategorii nie może przekraczać 100 znaków.");
            });
        }
    }
}
