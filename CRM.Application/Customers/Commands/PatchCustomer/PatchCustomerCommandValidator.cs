using CRM.Application.Customers.Validators.Update;
using CRM.Domain.Enums;
using FluentValidation;

namespace CRM.Application.Customers.Commands.PatchCustomer
{
    public class PatchCustomerCommandValidator : AbstractValidator<PatchCustomerCommand>
    {
        public PatchCustomerCommandValidator()
        {
            RuleFor(x => x.CifId).GreaterThan(0).WithMessage("Customer CIF ID is required");

            RuleFor(x => x)
            .Must(x => x.Name is not null 
            || x.Contacts is not null 
            || x.Addresses is not null 
            || x.Identifications is not null )
            .WithMessage("At least one section musts be provided for update");

            //If Name is provided
            When(x => x.Name is not null, () =>
            {
                RuleFor(x => x.Name!.FirstName).NotEmpty().WithMessage("First Name is required")
                .MaximumLength(100).WithMessage("First Name cannot exceed 100 characters");

                RuleFor(x => x.Name!.MiddleName).MaximumLength(100).WithMessage("Middle Name cannot exceed 100 characters");

                RuleFor(x => x.Name!.LastName).NotEmpty().WithMessage("Last Name is required")
                .MaximumLength(100).WithMessage("Last Name cannot exceed 100 characters");
            });

            // Contacts section - only validate if provided
            When(x => x.Contacts is not null, () =>
            {
                RuleFor(x => x.Contacts!).NotEmpty().WithMessage("Contact list cannot be empty when provided");

                RuleForEach(x => x.Contacts!).SetValidator(new UpdateContactDtoValidator());

                RuleFor(x => x.Contacts!)
                    .Must(c => c.Count(x => x.IsPrimary) == 1)
                    .WithMessage("Exactly one primary contact is required.")
                    .When(x => x.Contacts!.Any());

                RuleFor(x => x.Contacts!)
                    .Must(c => c.GroupBy(x => x.Phone).All(g => g.Count() == 1))
                    .WithMessage("Duplicate phone numbers are not allowed.")
                    .When(x => x.Contacts!.Any());

            });

            // Addresses section — only validate if provided
            When(x => x.Addresses is not null, () =>
            {
                RuleFor(x => x.Addresses!).NotEmpty().WithMessage("Addresses list cannot be empty when provided.");

                RuleForEach(x => x.Addresses!).SetValidator(new UpdateAddressDtoValidator());

                RuleFor(x => x.Addresses!)
                    .Must(a => a.Count(x => x.IsPrimary) == 1)
                    .WithMessage("Exactly one primary address is required.")
                    .When(x => x.Addresses!.Any());

                RuleFor(x => x.Addresses!)
                    .Must(a => a.GroupBy(x => new { x.Street, x.City, x.Country })
                        .All(g => g.Count() == 1))
                    .WithMessage("Duplicate addresses are not allowed.")
                    .When(x => x.Addresses!.Any());
            });

            // Identifications section — only validate if provided
            When(x => x.Identifications is not null, () =>
            {
                RuleFor(x => x.Identifications!)
                    .NotEmpty().WithMessage("Identifications list cannot be empty when provided.");

                RuleForEach(x => x.Identifications!).SetValidator(new UpdateIdentificationDtoValidator());
                
                RuleFor(x => x.Identifications!)
                    .Must(i => i.GroupBy(x => x.DocumentNumber).All(g => g.Count() == 1))
                    .WithMessage("Duplicate document numbers are not allowed.")
                    .When(x => x.Identifications!.Any());

            });

            When(x => x.Profile is not null, () =>
            {
                RuleFor(x => x.Profile!.CustomerStatus)
                    .Must(BeValidEnum<CustomerStatus>)
                    .When(x => !string.IsNullOrWhiteSpace(x.Profile!.CustomerStatus))
                    .WithMessage("Invalid customer status.");

                RuleFor(x => x.Profile!.KycStatus)
                    .Must(BeValidEnum<KycStatus>)
                    .When(x => !string.IsNullOrWhiteSpace(x.Profile!.KycStatus))
                    .WithMessage("Invalid KYC status.");

                RuleFor(x => x.Profile!.RiskLevel)
                    .Must(BeValidEnum<RiskLevel>)
                    .When(x => !string.IsNullOrWhiteSpace(x.Profile!.RiskLevel))
                    .WithMessage("Invalid risk level.");

                RuleFor(x => x.Profile!.AnnualIncome)
                    .GreaterThanOrEqualTo(0)
                    .When(x => x.Profile!.AnnualIncome.HasValue);

                RuleFor(x => x.Profile!.ExpectedMonthlyTransactionVolume)
                    .GreaterThanOrEqualTo(0)
                    .When(x => x.Profile!.ExpectedMonthlyTransactionVolume.HasValue);
            });
        }

        private static bool BeValidEnum<TEnum>(string? value)
            where TEnum : struct, Enum
            => Enum.TryParse<TEnum>(value, true, out _);
    }
}
