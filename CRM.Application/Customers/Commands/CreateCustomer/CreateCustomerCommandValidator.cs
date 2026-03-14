using CRM.Application.Customers.Commands.CreateCustomer;
using CRM.Domain.Enums;
using FluentValidation;
using Microsoft.IdentityModel.Tokens;

public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerCommandValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100).WithMessage("First name is required.");
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(100).WithMessage("Last name is required.");

        RuleFor(x => x.Contacts).NotEmpty().WithMessage("At least one contact is required.");
        RuleFor(x => x.Addresses).NotEmpty().WithMessage("At least one address is required.");
        RuleFor(x => x.Identifications).NotEmpty().WithMessage("At least one identification is required.");
        RuleFor(x => x.Contacts.Count(c => c.IsPrimary))
            .Equal(1).WithMessage("Exactly one primary contact is required.");

        RuleFor(x => x.Addresses.Count(a => a.IsPrimary))
            .Equal(1).WithMessage("Exactly one primary address is required.");

        RuleForEach(x => x.Contacts).ChildRules(contact =>
        {
            contact.RuleFor(c => c.Type).NotEmpty().Must(type => IsValidContactType(type)).WithMessage("Invalid contact type.");
            contact.RuleFor(c => c.Type).NotEmpty().WithMessage("Contact type is required.");
            contact.RuleFor(c => c.Phone).NotEmpty().WithMessage("Contact phone is required.");
            contact.RuleFor(c => c.Email).EmailAddress().WithMessage("Contact email must be a valid email address.");           
            contact.RuleFor(c => c.ValidTo).GreaterThan(c => c.ValidFrom).When(c => c.ValidFrom.HasValue && c.ValidTo.HasValue).WithMessage("ValidTo must be after ValidFrom.");
        });

        RuleForEach(x => x.Addresses).ChildRules(address =>
        {
            address.RuleFor(a => a.Type).NotEmpty().Must(type => IsValidAddressType(type)).WithMessage("Invalid address type.");
            address.RuleFor(a => a.Street).NotEmpty().WithMessage("Address street is required.");
            address.RuleFor(a => a.City).NotEmpty().WithMessage("Address city is required.");
            address.RuleFor(a => a.State).NotEmpty().WithMessage("Address state is required.");
            address.RuleFor(a => a.ZipCode).NotEmpty().WithMessage("Address postal code is required.");
            address.RuleFor(a => a.Country).NotEmpty().WithMessage("Address country is required.");

            address.RuleFor(a => a.ValidTo)
            .GreaterThan(a => a.ValidFrom)
            .When(a => a.ValidFrom.HasValue && a.ValidTo.HasValue)
            .WithMessage("ValidTo must be after ValidFrom.");
        });

        RuleForEach(x => x.Identifications).ChildRules(id =>
        {
            id.RuleFor(i => i.Type).NotEmpty().Must(type => IsValidDocumentType(type)).WithMessage("Invalid document type.");
            id.RuleFor(i => i.Type).NotEmpty().WithMessage("Identification type is required.");
            id.RuleFor(i => i.DocumentNumber).NotEmpty().WithMessage("Identification number is required.");
            id.RuleFor(i => i.IssuingAuthority).NotEmpty().WithMessage("Identification issuing authority is required.");
            id.RuleFor(i => i.IssuedDate).LessThan(i => i.ExpiryDate).When(i => i.IssuedDate.HasValue && i.ExpiryDate.HasValue)            .WithMessage("Issued date must be before expiry date.");
        });
    


    }

    private static bool IsValidContactType(string value) =>
        Enum.TryParse<ContactType>(value, out _);
    private static bool IsValidAddressType(string value) =>
        Enum.TryParse<AddressType>(value, out _);
    private static bool IsValidDocumentType(string value) =>
        Enum.TryParse<DocumentType>(value, out _);         

}
