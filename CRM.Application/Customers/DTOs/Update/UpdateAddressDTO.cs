using CRM.Application.Customers.DTOs;

namespace CRM.Application.Customers.DTOs.Update
{
    public record UpdateAddressDto(
        Guid? Id,
        string Type,
        string Street,
        string City,
        string State,
        string ZipCode,
        string Country,
        bool IsPrimary,
        DateTime? ValidFrom = null,
        DateTime? ValidTo = null
    ): AddressDto(Type, Street, City, State, ZipCode, Country,IsPrimary,ValidFrom, ValidTo);
}