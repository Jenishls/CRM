using CRM.Contracts.Requests.Common;

namespace CRM.API.Contracts.Requests.Customers
{
    public record PatchCustomerRequest(
    NameRequestModel? Name,                         // ← null = don't touch
    List<ContactRequestModel>? Contacts,            // ← null = don't touch
    List<AddressRequestModel>? Addresses,           // ← null = don't touch
    List<IdentificationRequestModel>? Identifications);
    
}