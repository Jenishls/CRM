using MediatR;
using ErrorOr;
using CRM.Application.Customers.DTOs;
using CRM.Application.Customers.Commands.Common;

namespace CRM.Application.Customers.Commands.CreateCustomer
{
    public class CreateCustomerCommand : ICustomerCommand, IRequest<ErrorOr<Guid>>
    {
        public string FirstName { get; set; } = default!;
        public string? MiddleName { get; set; }
        public string LastName { get; set; } = default!;
        public List<ContactDto> Contacts { get; set; } = new();
        public List<AddressDto> Addresses { get; set; } = new();
        public List<IdentificationDto> Identifications { get; set; } = new();
    }
}