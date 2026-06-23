using CRM.Contracts.Requests.Common;

namespace CRM.API.Contracts.Requests.Customers;

public class CreateCustomerRequest
{
    public string FirstName { get; init; } = default!;
    public string? MiddleName { get; init; }
    public string LastName { get; init; } = default!;
    public List<ContactRequest> Contacts { get; init; } = new();

    public List<AddressRequest> Addresses { get; init; } = new();

    public List<IdentityDocumentRequest> IdentityDocuments { get; init; } = new();

    public PersonalProfileRequestModel? Profile { get; init; }
}

public sealed class ContactRequest
{
    public string Type { get; init; } = default!; // Homw / Work

    public string Email { get; init; } = default!;

    public string Phone { get; init; } = default!;

    public bool IsPrimary { get; init; }
}

public sealed class AddressRequest
{
    public string Type { get; init; } = default!; 
    public string AddressLine1 { get; init; } = default!;

    public string? AddressLine2 { get; init; }

    public string City { get; init; } = default!;

    public string State { get; init; } = default!;

    public string PostalCode { get; init; } = default!;

    public string Country { get; init; } = default!;

    public bool IsPrimary { get; init; }
}

public sealed class IdentityDocumentRequest
{
    public string Type { get; init; } = default!; // Passport / NationalId

    public string DocumentNumber { get; init; } = default!;

    public string IssuingAuthority { get; init; } = default!;

    public string IssuingCountry { get; init; } = default!;

    public DateTime IssuedDate { get; init; }

    public DateTime? ExpiryDate { get; init; }

    public bool IsPrimary { get; init; }
}
