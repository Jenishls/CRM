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

}