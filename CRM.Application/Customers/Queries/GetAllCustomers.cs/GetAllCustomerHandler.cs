using CRM.Application.Common.Errors;
using CRM.Application.Customers.Queries.GetAllCustomers;
using CRM.Contracts.Responses;
using ErrorOr;
using MediatR;

namespace Crm.Application.Customers.Queries.GetAllCustomers;

public class GetAllCustomerHandler : IRequestHandler<GetAllCustomersQuery, ErrorOr<CustomerListResponse>>
{
    private readonly ICustomerRepository _repository;

    public GetAllCustomerHandler(ICustomerRepository repository)
    {
        _repository = repository;
    }

    public async Task<ErrorOr<CustomerListResponse>> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
    {
        var customers = await _repository.GetAllAsync(cancellationToken);

        if(customers is null || customers.Count == 0)
        {
            return CustomerErrors.NoCustomerFound;
        }
        
        var response = customers.Select(c => new CustomerResponse
        {
            CifId = c.CifId,
            Id = c.Id.Value,
            FirstName = c.FullName.First,
            MiddleName = c.FullName.Middle,
            LastName = c.FullName.Last,
            PrimaryEmail = c.Contacts.Where(x=>x.IsPrimary).Select(x=>x.Email).FirstOrDefault(),
            PrimaryPhone = c.Contacts.Where(x=>x.IsPrimary).Select(x=>x.Phone).FirstOrDefault(),
            IsActive = c.IsActive
        }).ToList();
        
        return new CustomerListResponse
        {
            Customers = response
        };

    }
}