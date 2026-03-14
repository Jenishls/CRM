using MediatR;
using ErrorOr;
using CRM.Domain.Enums;


namespace CRM.Application.Customers.Commands.CreateCustomer
{
    public class CreateCustomerCommand : IRequest<ErrorOr<Guid>>
    {
        public string FirstName { get; set; } = default!;
        public string? MiddleName { get; set; }
        public string LastName { get; set; } = default!;
        public List<ContactModel> Contacts { get; set; } = new();
        public List<AddressModel> Addresses { get; set; } = new();
        public List<IdentificationModel> Identifications { get; set; } = new();

        public record ContactModel(
            string Type,
            string Phone,
            string Email,
            bool IsPrimary,
            DateTime? ValidFrom = null,
            DateTime? ValidTo = null);
        public record AddressModel(string Type,
                                   string Street,
                                   string City,
                                   string State,
                                   string ZipCode,
                                   string Country,
                                   bool IsPrimary,
                                   DateTime? ValidFrom = null,
                                   DateTime? ValidTo = null);
        public record IdentificationModel(string Type,
                                          string DocumentNumber,
                                          string IssuingAuthority,
                                          string IssuingCountry,
                                          DateTime? IssuedDate = null,
                                          DateTime? ExpiryDate = null);
    }
}