using FluentValidation;

namespace Application.Features.Users.Dtos
{
    public class UpdateUserDtoValidator : AbstractValidator<UpdateUserDto>
    {
        public UpdateUserDtoValidator()
        {
            When(dto => !string.IsNullOrEmpty(dto.FirstName), () =>
            {
                RuleFor(dto => dto.FirstName)
                    .MaximumLength(50).WithMessage("Imię nie może być dłuższe niż 50 znaków.");
            });

            When(dto => !string.IsNullOrEmpty(dto.LastName), () =>
            {
                RuleFor(dto => dto.LastName)
                    .MaximumLength(50).WithMessage("Nazwisko nie może być dłuższe niż 50 znaków.");
            });


            When(dto => !string.IsNullOrEmpty(dto.NewPassword), () =>
            {
                RuleFor(dto => dto.OldPassword)
                    .MinimumLength(8).WithMessage("Nowe hasło musi mieć co najmniej 8 znaków.");
                RuleFor(dto => dto.NewPassword)
                    .MinimumLength(8).WithMessage("Nowe hasło musi mieć co najmniej 8 znaków.");
                RuleFor(dto => dto.ConfirmNewPassword)
                    .Equal(dto => dto.NewPassword).WithMessage("Nowe hasło i jego potwierdzenie muszą być takie same.");
            });

            When(dto => dto.Duty.HasValue, () =>
            {
                RuleFor(dto => dto.Duty)
                    .IsInEnum().WithMessage("Unit must be a valid enum value.");
            });

            When(dto => !string.IsNullOrEmpty(dto.Pin), () =>
            {
                RuleFor(dto => dto.Pin)
                    .NotEmpty().WithMessage("PIN jest wymagany.")
                    .Length(4).WithMessage("PIN musi składać się z dokładnie 4 cyfr.")
                    .Matches("^[0-9]{4}$").WithMessage("PIN może zawierać tylko cyfry.");
            });
        }
    }
}