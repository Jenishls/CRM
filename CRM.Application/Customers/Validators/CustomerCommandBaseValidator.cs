using FluentValidation;
using CRM.Application.Customers.Commands.Common;
using System.Linq.Expressions;

namespace CRM.Application.Customers.Validators
{
    public abstract class CustomerCommonBaseValidator<T> : AbstractValidator<T>
    where T : ICustomerCommand
    {
        protected CustomerCommonBaseValidator()
        {
            RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(100).WithMessage("First name cannot exceed 100 characters.");

            RuleFor(x => x.MiddleName)
            .MaximumLength(100).WithMessage("Middle name cannot exceed 100 characters.")
            .When(x => !string.IsNullOrEmpty(x.MiddleName));  // ← only validate if provided

            RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(100).WithMessage("Last name cannot exceed 100 characters.");
   
        }

        protected void AddAddressCollectionRules<TAddress>(
            Expression<Func<T, IEnumerable<TAddress>>> selector,
            Func<TAddress, bool> isPrimarySelector
        )
        {
            RuleFor(selector)
            .NotEmpty().WithMessage("At least one address is required.");

            RuleFor(selector)
            .Must(a => a.Count(isPrimarySelector) == 1)
            .WithMessage("Exactly one primary address is required.");
        }

        protected void AddContactCollectionRules<TContact>(
            Expression<Func<T, IEnumerable<TContact>>> selector, 
            Func<TContact, bool> isPrimarySelector
        )
        {
            RuleFor(selector)
            .NotEmpty().WithMessage("At least one contact is required.");

            RuleFor(selector)
            .Must(a => a.Count(isPrimarySelector) == 1)
            .WithMessage("Exactly one primary contact is required.");
        }

        protected void AddIdentificationRules<TIdentification>(
            Expression<Func<T, IEnumerable<TIdentification>>> selector
        )
        {
            RuleFor(selector)
            .NotEmpty().WithMessage("At least one identification is required.");
        }
    }
}