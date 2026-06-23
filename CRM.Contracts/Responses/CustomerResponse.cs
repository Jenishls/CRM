namespace CRM.Contracts.Responses;

public class CustomerResponse
{
    public int CifId { get; init; }
    public Guid Id { get; init; }
    public string FirstName { get; init; } = default!;
    public string? MiddleName { get; init; }
    public string LastName { get; init; } = default!;
    public string? PrimaryEmail { get; init; }
    public string? PrimaryPhone { get; init; }
    public bool IsActive { get; init; }
    public string CustomerType { get; init; } = default!;
    public string CustomerStatus { get; init; } = default!;
    public string KycStatus { get; init; } = default!;
    public string RiskLevel { get; init; } = default!;
    public string? RiskCategory { get; init; }
    public string? RiskSubCategory { get; init; }
    public decimal? AnnualIncome { get; init; }
    public string? SourceOfFunds { get; init; }
    public string? Occupation { get; init; }
    public string? EmployerName { get; init; }
    public string? EmploymentStatus { get; init; }
    public string? PurposeOfRelationship { get; init; }
    public decimal? ExpectedMonthlyTransactionVolume { get; init; }
    public DateTime CustomerSince { get; init; }

}
