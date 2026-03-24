using CRM.Application.Customers.DTOs;

namespace Crm.Application.Customers.Dtos.Update
{
    public record UpdateIdentificationDto(
        Guid? Id,
        string Type,
        string DocumentNumber,
        string IssuingAuthority,
        string IssuingCountry,
        DateTime IssuedDate,
        DateTime? ExpiryDate = null
    ):IdentificationDto(Type, DocumentNumber, IssuingAuthority, IssuingCountry, IssuedDate, ExpiryDate);
}