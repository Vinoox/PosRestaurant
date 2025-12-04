using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Orders.Dtos.Commands;
using Domain.Enums;
using FluentValidation;

namespace Application.Features.Orders.Dtos.Validators
{
    public class CreateOrderDtoValidator : AbstractValidator<CreateOrderDto>
    {
        public CreateOrderDtoValidator()
        {
            RuleFor(x => x.Type)
                .IsInEnum().WithMessage("Nieprawidłowy typ zamówienia.");

            RuleFor(x => x.Items)
                .NotEmpty().WithMessage("Zamówienie musi zawierać pozycje.");

            RuleForEach(x => x.Items).ChildRules(item =>
            {
                item.RuleFor(i => i.ProductId).GreaterThan(0);
                item.RuleFor(i => i.Quantity)
                    .GreaterThan(0).WithMessage("Ilość produktu musi być dodatnia.");
            });

            RuleFor(x => x.TargetCompletionDate)
                .Must(BeValidFutureDate)
                .When(x => x.TargetCompletionDate.HasValue)
                .WithMessage("Data realizacji musi być w przyszłości.");


            When(x => x.Type == OrderType.Delivery, () =>
            {
                RuleFor(x => x.DeliveryAddress)
                    .NotNull().WithMessage("Adres dostawy jest wymagany.")
                    .SetValidator(new AddressDtoValidator()!);

                RuleFor(x => x.CustomerName)
                    .NotEmpty().WithMessage("Imię klienta jest wymagane przy dostawie.");

                RuleFor(x => x.PhoneNumber)
                    .NotEmpty().WithMessage("Numer telefonu jest wymagany przy dostawie.");
            });

            When(x => x.Type == OrderType.Takeaway, () =>
            {
                RuleFor(x => x.CustomerName)
                    .NotEmpty().WithMessage("Imię klienta jest wymagane przy odbiorze własnym.");

                RuleFor(x => x.DeliveryAddress)
                    .Null().WithMessage("Nie podawaj adresu dla odbioru własnego.");
            });
        }
        private bool BeValidFutureDate(DateTime? date)
        {
            return !date.HasValue || date.Value > DateTime.UtcNow;
        }
    }
    public class AddressDtoValidator : AbstractValidator<AddressDto>
    {
        public AddressDtoValidator()
        {
            RuleFor(x => x.Street).NotEmpty().WithMessage("Ulica jest wymagana.");
            RuleFor(x => x.City).NotEmpty().WithMessage("Miasto jest wymagane.");
            RuleFor(x => x.PostalCode).NotEmpty().WithMessage("Kod pocztowy jest wymagany.");
        }
    }
}
