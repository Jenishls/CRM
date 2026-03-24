using CRM.Application.Customers.Validators.Update;
using FluentValidation;

namespace CRM.Application.Customers.UpdateCustomer
{
    public class UpdateCustomerAddressesCommandValidator : AbstractValidator<UpdateCustomerAddressesCommand>
    {
        public UpdateCustomerAddressesCommandValidator()
        {
            RuleForEach(x => x.Addresses).SetValidator(new UpdateAddressDtoValidator());

            RuleFor(x => x.CifId).GreaterThan(0).WithMessage("Customer CIF Id is required");

            RuleFor(x => x.Addresses).NotEmpty().WithMessage("At least one address is required.");

            RuleFor(x => x.Addresses).Must(a => a.Count(x => x.IsPrimary) == 1)
                .WithMessage("Exactly one primary address is required.")
                .When(x => x.Addresses.Any());

            RuleFor(x => x.Addresses).Must(a => a.GroupBy(x => new { x.Street, x.City, x.Country })
                .All(g => g.Count() == 1))
                .WithMessage("Duplicate addresses are not allowed.")
                .When(x => x.Addresses.Any());
        }
    }
}