using Crm.Application.Customers.Dtos.Update;
using CRM.Application.Customers.DTOs.Update;
using ErrorOr;
using MediatR;

namespace CRM.Application.Customers.Commands.PatchCustomer
{
    public class PatchCustomerCommand : IRequest<ErrorOr<Unit>>
    {
        public int CifId {get;set;}
        public PatchNameDto? Name {get; set;}
        public List<UpdateContactDto>? Contacts {get; set;}
        public List<UpdateAddressDto>? Addresses {get; set;}
        public List<UpdateIdentificationDto>? Identifications {get; set;}
    }

    public record PatchNameDto(
        string FirstName,
        string? MiddleName,
        string LastName
    );

}