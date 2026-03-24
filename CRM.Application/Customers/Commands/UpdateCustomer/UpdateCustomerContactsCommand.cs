using Crm.Application.Customers.Dtos.Update;
using ErrorOr;
using MediatR;

namespace CRM.Application.Customers.UpdateCustomer
{
    public class UpdateCustomerContactsCommand : IRequest<ErrorOr<Unit>>
    {
        public int CifId { get; set; }
        public List<UpdateContactDto> Contacts { get; set; } = new();
    }

}
