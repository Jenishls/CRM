namespace CRM.Application.Customers.DTOs
{
    public record AddressDto(
        string Type,
        string Street,
        string City,
        string State,
        string ZipCode,
        string Country,
        bool IsPrimary,
        DateTime? ValidFrom = null,
        DateTime? ValidTo = null
    );
}