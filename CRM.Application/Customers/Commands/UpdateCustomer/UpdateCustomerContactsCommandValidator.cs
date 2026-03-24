using CRM.Application.Customers.UpdateCustomer;
using CRM.Application.Customers.Validators;
using CRM.Application.Customers.Validators.Update;
using FluentValidation;

namespace Crm.Application.Customers.Commands.UpdateCutomer
{
    public class UpdateCustomerContactsCommandValidator : AbstractValidator<UpdateCustomerContactsCommand>
    {
        public UpdateCustomerContactsCommandValidator()
        {
            RuleForEach(x=> x.Contacts).SetValidator(new UpdateContactDtoValidator());

            RuleFor(x=> x.CifId).GreaterThan(0).WithMessage("Cif Id is required for exisiting contacts");

            RuleFor(x => x.Contacts).NotEmpty().WithMessage("At least one contact is required.");

            RuleFor(x => x.Contacts).Must(c => c.Count(x => x.IsPrimary) == 1).WithMessage("Exactly one primary contact is required.")
            .When(x => x.Contacts.Any());

            RuleFor(x => x.Contacts).Must(c => c.GroupBy(x => x.Phone).All(g => g.Count() == 1))
            .WithMessage("Duplicate phone numbers are not allowed.")
            .When(x => x.Contacts.Any());

            RuleFor(x => x.Contacts).Must(c => c.Where(x => !string.IsNullOrEmpty(x.Email))
            .GroupBy(x => x.Email).All(g => g.Count() == 1))
            .WithMessage("Duplicate email addresses are not allowed.")
            .When(x => x.Contacts.Any());
        }
    }
}