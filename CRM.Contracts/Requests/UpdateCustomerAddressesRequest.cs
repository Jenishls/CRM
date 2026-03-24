using CRM.Contracts.Requests.Common;

namespace CRM.API.Contracts.Requests.Customers
{
    public record UpdateCustomerAddressesRequest(
    List<AddressRequestModel> Addresses);
}