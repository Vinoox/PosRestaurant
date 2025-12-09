using FluentValidation;
using Application.Features.Auth.Commands;

namespace Application.Features.Auth.Dtos.Validators
{
    public class AuthenticateDtoValidator : AbstractValidator<AuthenticateDto>
    {
        public AuthenticateDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email jest wymagany.")
                .EmailAddress().WithMessage("Niepoprawny format adresu email.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Hasło jest wymagane.");
        }
    }
}