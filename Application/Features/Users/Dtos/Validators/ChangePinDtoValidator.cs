using Application.Features.Users.Dtos.Commands;
using FluentValidation;

namespace Application.Features.Users.Dtos.Validators
{
    public class ChangePinDtoValidator : AbstractValidator<ChangePinDto>
    {
        public ChangePinDtoValidator()
        {
            RuleFor(x => x.OldPin)
                .NotEmpty().WithMessage("Stary PIN jest wymagany.")
                .Length(4).WithMessage("Stary PIN musi składać się z dokładnie 4 cyfr.")
                .Matches("^[0-9]{4}$").WithMessage("Stary PIN może zawierać tylko cyfry.");

            RuleFor(x => x.NewPin)
                .NotEmpty().WithMessage("Nowy PIN jest wymagany.")
                .Length(4).WithMessage("Nowy PIN musi składać się z dokładnie 4 cyfr.")
                .Matches("^[0-9]{4}$").WithMessage("Nowy PIN może zawierać tylko cyfry.")
                .NotEqual(x => x.OldPin).WithMessage("Nowy PIN musi być inny niż stary.");

           RuleFor(x => x.ConfirmNewPin)
                .NotEmpty().WithMessage("Potwierdzenie nowego PINu jest wymagane.")
                .Equal(x => x.NewPin).WithMessage("Podane PINy muszą być takie same.");
        }
    }
}