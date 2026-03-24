namespace CRM.Contracts.Requests.Common
{
    public record NameRequestModel(
    string FirstName,
    string? MiddleName,
    string LastName);
}