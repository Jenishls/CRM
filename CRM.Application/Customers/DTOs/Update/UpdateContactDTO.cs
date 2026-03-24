using CRM.Application.Customers.DTOs;

namespace Crm.Application.Customers.Dtos.Update
{
    public record UpdateContactDto(
        Guid? Id,
        string Type,
        string Phone,
        string? Email,
        bool IsPrimary,
        DateTime? ValidFrom = null,
        DateTime? ValidTo = null
    ):ContactDto( Type, Phone, Email, IsPrimary, ValidFrom, ValidTo);
}