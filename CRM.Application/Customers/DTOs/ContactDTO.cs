namespace CRM.Application.Customers.DTOs
{
    public record ContactDto(
        string Type,
        string Phone,
        string? Email,
        bool IsPrimary,
        DateTime? ValidFrom = null,
        DateTime? ValidTo = null
    );
}