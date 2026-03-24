using CRM.Application.Customers.Validators;
using CRM.Application.Customers.Commands.CreateCustomer;
using FluentValidation;

public class CreateCustomerCommandValidator : CustomerCommonBaseValidator<CreateCustomerCommand>
{
    public CreateCustomerCommandValidator()
    {
        RuleFor(x => x.Contacts).NotEmpty().WithMessage("At least one contact is required.");
        RuleFor(x => x.Addresses).NotEmpty().WithMessage("At least one address is required.");

        AddAddressCollectionRules(x => x.Contacts, c => c.IsPrimary);
        AddContactCollectionRules(x => x.Addresses, a => a.IsPrimary);
        AddIdentificationRules(x => x.Identifications);

        RuleForEach(x=>x.Addresses).SetValidator(new AddressDtoValidator());
        RuleForEach(x=>x.Contacts).SetValidator(new ContactDtoValidator());
        RuleForEach(x=>x.Identifications).SetValidator(new IdentificationDtoValidator());
    }   
}