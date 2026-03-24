using CRM.Application.Customers.Validators.Update;
using FluentValidation;

namespace CRM.Application.Customers.UpdateCustomer
{
    public class UpdateCustomerIdentificationsCommandValidator : AbstractValidator<UpdateCustomerIdentificationsCommand>
    {
        public UpdateCustomerIdentificationsCommandValidator()
        {
            RuleForEach(x => x.Identifications).SetValidator(new UpdateIdentificationDtoValidator());

            RuleFor(x => x.CifId).GreaterThan(0).WithMessage("Customer CIF ID is required.");

            RuleFor(x => x.Identifications).NotEmpty().WithMessage("At least one identification is required.");

            RuleFor(x => x.Identifications).Must(i => i.GroupBy(x => x.DocumentNumber).All(g => g.Count() == 1))
                .WithMessage("Duplicate document numbers are not allowed.")
                .When(x => x.Identifications.Any());
        }
    }
}