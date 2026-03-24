using CRM.Contracts.Requests.Common;

namespace CRM.API.Contracts.Requests.Customers
{
    public record UpdateCustomerContactsRequest(
    List<ContactRequestModel> Contacts);
}