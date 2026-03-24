using Crm.Application.Customers.Dtos.Update;
using ErrorOr;
using MediatR;

namespace CRM.Application.Customers.UpdateCustomer
{
    public class UpdateCustomerIdentificationsCommand : IRequest<ErrorOr<Unit>>
    {
        public int CifId { get; set; }
        public List<UpdateIdentificationDto> Identifications { get; set; } = new();
    }
}