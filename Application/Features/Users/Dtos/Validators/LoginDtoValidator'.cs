using Application.Features.Users.Dtos.Commands;
using FluentValidation;

namespace Application.Features.Users.Dtos.Validators
{
    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email jest wymagany.")
                .EmailAddress().WithMessage("Podano niepoprawny format adresu email.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Hasło jest wymagane.");

            RuleFor(x => x.RestaurantId)
                .NotEmpty().WithMessage("Id restauracji jest wymagane.")
                .GreaterThan(0).WithMessage("Id restauracji musi być większe od zera.");
        }
    }
}