namespace CRM.Contracts.Requests.Common
{
    public record AddressRequestModel(
        Guid? Id,
        string Type,
        string Street,
        string City,
        string State,
        string ZipCode,
        string Country,
        bool IsPrimary,
        DateTime? ValidFrom = null,
        DateTime? ValidTo = null);
}