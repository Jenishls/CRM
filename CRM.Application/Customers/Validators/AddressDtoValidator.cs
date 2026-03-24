using CRM.Application.Customers.DTOs;
using CRM.Domain.Enums;
using FluentValidation;

namespace CRM.Application.Customers.Validators
{
    public class AddressDtoValidator<T> : AbstractValidator<T> 
    where T : AddressDto
    {
        public AddressDtoValidator()
        {
            RuleFor(a => a.Type).NotEmpty().Must(type => IsValidAddressType(type)).WithMessage("Invalid address type.");
            RuleFor(a => a.Street).NotEmpty().WithMessage("Address street is required.");
            RuleFor(a => a.City).NotEmpty().WithMessage("Address city is required.");
            RuleFor(a => a.State).NotEmpty().WithMessage("Address state is required.");
            RuleFor(a => a.ZipCode).NotEmpty().WithMessage("Address postal code is required.");
            RuleFor(a => a.Country).NotEmpty().WithMessage("Address country is required.");

            RuleFor(a => a.ValidTo)
            .GreaterThan(a => a.ValidFrom)
            .When(a => a.ValidFrom.HasValue && a.ValidTo.HasValue)
            .WithMessage("ValidTo must be after ValidFrom.");
        }

        private static bool IsValidAddressType(string value) =>
            Enum.TryParse<AddressType>(value, out _);
            
    }

    public class AddressDtoValidator : AddressDtoValidator<AddressDto> { }

}