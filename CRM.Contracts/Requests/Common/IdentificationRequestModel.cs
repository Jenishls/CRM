namespace CRM.Contracts.Requests.Common
{
        public record IdentificationRequestModel(
        Guid? Id,
        string Type,
        string DocumentNumber,
        string IssuingAuthority,
        string IssuingCountry,
        DateTime IssuedDate ,
        DateTime? ExpiryDate = null);
}