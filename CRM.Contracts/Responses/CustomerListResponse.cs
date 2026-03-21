using CRM.Contracts.Responses;

namespace CRM.Contracts.Responses;

public class CustomerListResponse
{
    public List<CustomerResponse> Customers { get; init; } = new();
}