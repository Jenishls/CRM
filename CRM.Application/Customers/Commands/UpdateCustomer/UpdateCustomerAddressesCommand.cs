using CRM.Application.Customers.DTOs.Update;
using ErrorOr;
using MediatR;

namespace CRM.Application.Customers.UpdateCustomer
{
    public class UpdateCustomerAddressesCommand : IRequest<ErrorOr<Unit>>
    {
        public int CifId { get; set; }
        public List<UpdateAddressDto> Addresses { get; set; } = new();
    }
}