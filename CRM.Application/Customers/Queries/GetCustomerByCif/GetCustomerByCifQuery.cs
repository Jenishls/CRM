using CRM.Contracts.Responses;
using ErrorOr;
using MediatR;

namespace CRM.Application.Customers.Queries.GetCustomerByCif;

public class GetCustomerByCifQuery : IRequest<ErrorOr<CustomerDetailResponse?>>
{
    public int CifId { get; init; }

    public GetCustomerByCifQuery(int cifId)
    {
        CifId = cifId;
    }
}