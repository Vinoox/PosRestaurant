using FluentValidation;
using Identity.Application.Auth.Commands;

namespace Identity.Application.Auth.Validators
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