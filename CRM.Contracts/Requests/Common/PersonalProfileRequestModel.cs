namespace CRM.Contracts.Requests.Common
{
    public sealed class PersonalProfileRequestModel
    {
        public string? CustomerStatus { get; init; }
        public string? KycStatus { get; init; }
        public string? RiskLevel { get; init; }
        public string? RiskCategory { get; init; }
        public string? RiskSubCategory { get; init; }
        public decimal? AnnualIncome { get; init; }
        public string? SourceOfFunds { get; init; }
        public string? Occupation { get; init; }
        public string? EmployerName { get; init; }
        public string? EmploymentStatus { get; init; }
        public string? PurposeOfRelationship { get; init; }
        public decimal? ExpectedMonthlyTransactionVolume { get; init; }
        public DateTime? CustomerSince { get; init; }
    }
}
