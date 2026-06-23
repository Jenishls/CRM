namespace CRM.Contracts.Responses
{
    public class CustomerDetailResponse
    {
        public Guid Id { get; init; }
        public int CifId { get; init; }
        public string FirstName { get; init; } = default!;
        public string? MiddleName { get; init; }
        public string LastName { get; init; } = default!;
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
        public List<ContactResponse> Contacts { get; init; } = new();
        public List<AddressResponse> Addresses { get; init; } = new();
        public List<IdentityDocumentResponse> IdentityDocuments { get; init; } = new();

        
        public sealed class ContactResponse
        {
            public Guid Id { get; set; }
            public string Type { get; init; } = default!; // Homw / Work
            public string? Email { get; init; } = default!;
            public string Phone { get; init; } = default!;
            public bool IsPrimary { get; init; }
        }

        public sealed class AddressResponse
        {
            public Guid Id { get; set; }
            public string Type { get; init; } = default!; 
            public string Street { get; init; } = default!;
            public string City { get; init; } = default!;
            public string State { get; init; } = default!;
            public string ZipCode { get; init; } = default!;
            public string Country { get; init; } = default!;
            public bool IsPrimary { get; init; }
        }

        public sealed class IdentityDocumentResponse
        {
            public Guid Id { get; set; }
            public string Type { get; init; } = default!; // Passport / NationalId
            public string DocumentNumber { get; init; } = default!;
            public string IssuingAuthority { get; init; } = default!;
            public string IssuingCountry { get; init; } = default!;
            public DateTime? IssuedDate { get; init; }
            public DateTime? ExpiryDate { get; init; }
            public bool IsPrimary { get; init; }
        }
    }
}
