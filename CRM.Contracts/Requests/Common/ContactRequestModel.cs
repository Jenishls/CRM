namespace CRM.Contracts.Requests.Common
{
    public record ContactRequestModel(
        Guid? Id,
        string Type,
        string Phone,
        string Email,
        bool IsPrimary,
        DateTime? ValidFrom = null,
        DateTime? ValidTo = null);
}