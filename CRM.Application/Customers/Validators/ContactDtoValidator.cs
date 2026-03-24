using CRM.Application.Customers.DTOs;
using CRM.Application.Customers.Validators;
using CRM.Domain.Enums;
using FluentValidation;

namespace CRM.Application.Customers.Validators
{
    
public class ContactDtoValidator<T> : AbstractValidator<T>
where T : ContactDto
{
    public ContactDtoValidator()
    {
        RuleFor(c => c.Type)
            .NotEmpty().WithMessage("Contact type is required.")
            .Must(IsValidContactType).WithMessage("Invalid contact type.");

        RuleFor(c => c.Phone)
            .NotEmpty().WithMessage("Contact phone is required.");

        RuleFor(c => c.Email)
            .EmailAddress().WithMessage("Contact email must be a valid email address.");

        RuleFor(c => c.ValidTo)
            .GreaterThan(c => c.ValidFrom)
            .When(c => c.ValidFrom.HasValue && c.ValidTo.HasValue)
            .WithMessage("ValidTo must be after ValidFrom.");
    }

    private static bool IsValidContactType(string value) =>
        Enum.TryParse<ContactType>(value, out _);
    }
}

public class ContactDtoValidator : ContactDtoValidator<ContactDto> { }