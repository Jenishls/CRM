using ErrorOr;
using MediatR;

namespace CRM.Application.Customers.UpdateCustomer
{
    public class UpdateCustomerNameCommand : IRequest<ErrorOr<Unit>>
    {
        public int CifId { get; set; }
        public string FirstName { get; set; } = default!;
        public string? MiddleName { get; set; }
        public string LastName { get; set; } = default!;
    }
}