using MediatR;
using CRM.Contracts.Responses;
using ErrorOr;

namespace CRM.Application.Customers.Queries.GetAllCustomers;

    public record GetAllCustomersQuery : IRequest<ErrorOr<CustomerListResponse>>;
