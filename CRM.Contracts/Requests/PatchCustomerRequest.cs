using CRM.Contracts.Requests.Common;

namespace CRM.API.Contracts.Requests.Customers
{
    public record PatchCustomerRequest(
        NameRequestModel? Name,
        List<ContactRequestModel>? Contacts,
        List<AddressRequestModel>? Addresses,
        List<IdentificationRequestModel>? Identifications,
        PersonalProfileRequestModel? Profile);
}
