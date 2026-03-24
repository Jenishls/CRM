namespace CRM.Application.Customers.DTOs
{
    public record IdentificationDto(
        string Type,
        string DocumentNumber,
        string IssuingAuthority,
        string IssuingCountry,
        DateTime IssuedDate,
        DateTime? ExpiryDate = null
    );
}