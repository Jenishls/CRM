using CRM.Application.Customers.Validators;
using CRM.Application.Customers.Commands.CreateCustomer;
using CRM.Domain.Enums;
using FluentValidation;

public class CreateCustomerCommandValidator : CustomerCommonBaseValidator<CreateCustomerCommand>
{
    public CreateCustomerCommandValidator()
    {
        RuleFor(x => x.Contacts).NotEmpty().WithMessage("At least one contact is required.");
        RuleFor(x => x.Addresses).NotEmpty().WithMessage("At least one address is required.");

        AddContactCollectionRules(x => x.Contacts, c => c.IsPrimary);
        AddAddressCollectionRules(x => x.Addresses, a => a.IsPrimary);
        AddIdentificationRules(x => x.Identifications);

        RuleForEach(x=>x.Addresses).SetValidator(new AddressDtoValidator());
        RuleForEach(x=>x.Contacts).SetValidator(new ContactDtoValidator());
        RuleForEach(x=>x.Identifications).SetValidator(new IdentificationDtoValidator());

        RuleFor(x => x.CustomerStatus)
            .Must(BeValidEnum<CustomerStatus>)
            .When(x => !string.IsNullOrWhiteSpace(x.CustomerStatus))
            .WithMessage("Invalid customer status.");

        RuleFor(x => x.KycStatus)
            .Must(BeValidEnum<KycStatus>)
            .When(x => !string.IsNullOrWhiteSpace(x.KycStatus))
            .WithMessage("Invalid KYC status.");

        RuleFor(x => x.RiskLevel)
            .Must(BeValidEnum<RiskLevel>)
            .When(x => !string.IsNullOrWhiteSpace(x.RiskLevel))
            .WithMessage("Invalid risk level.");

        RuleFor(x => x.AnnualIncome)
            .GreaterThanOrEqualTo(0)
            .When(x => x.AnnualIncome.HasValue);

        RuleFor(x => x.ExpectedMonthlyTransactionVolume)
            .GreaterThanOrEqualTo(0)
            .When(x => x.ExpectedMonthlyTransactionVolume.HasValue);
    }   

    private static bool BeValidEnum<TEnum>(string? value)
        where TEnum : struct, Enum
        => Enum.TryParse<TEnum>(value, true, out _);
}
