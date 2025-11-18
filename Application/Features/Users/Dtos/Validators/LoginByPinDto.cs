using Application.Features.Users.Dtos.Commands;
using FluentValidation;

namespace Application.Features.Users.Dtos.Validators
{
    public class LoginByPinDtoValidator : AbstractValidator<LoginByPinDto>
    {
        public LoginByPinDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email jest wymagany.")
                .EmailAddress().WithMessage("Podano niepoprawny format adresu email.");

            RuleFor(x => x.Pin)
                .NotEmpty().WithMessage("PIN jest wymagany.")
                .Length(4).WithMessage("PIN musi składać się z dokładnie 4 cyfr.")
                .Matches("^[0-9]{4}$").WithMessage("PIN może zawierać tylko cyfry.");
        }
    }
}