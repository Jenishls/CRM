using CRM.Domain.Enums;
using CRM.Domain.Common;

namespace CRM.Domain.Entities
{
    public sealed class IdentityDocument
    {
        public Guid Id { get; private set; }
        public DocumentType Type { get; private set; }
        public string DocumentNumber { get; private set; }
        public string IssuingAuthority { get; private set; }
        public string IssuingCountry { get; private set; }
        public DateTime? IssueDate { get; private set; }        // ← nullable, may not be known
        public DateTime? ExpiryDate { get; private set; }
        public string? FileReference { get; private set; }

        private IdentityDocument() { }                         // ← EF Core

        private IdentityDocument(
            Guid id,                                            // ← added
            DocumentType type,
            string documentNumber,
            string issuingAuthority,
            string issuingCountry,
            DateTime? issueDate,                                
            DateTime? expiryDate,
            string? fileReference)
        {
            Id = id;                                            
            Type = type;
            DocumentNumber = documentNumber.Trim();
            IssuingAuthority = issuingAuthority.Trim();
            IssuingCountry = issuingCountry.Trim();
            IssueDate = issueDate;
            ExpiryDate = expiryDate;
            FileReference = fileReference?.Trim();
        }

        public static IdentityDocument Create(
            DocumentType type,
            string documentNumber,
            string issuingAuthority,
            string issuingCountry,
            DateTime? issueDate = null,                         
            DateTime? expiryDate = null,
            string? fileReference = null)
        {
            // ← removed redundant enum check
            if (string.IsNullOrWhiteSpace(documentNumber))
                throw new DomainException("Document number is required.");

            if (string.IsNullOrWhiteSpace(issuingAuthority))
                throw new DomainException("Issuing authority is required.");

            if (string.IsNullOrWhiteSpace(issuingCountry))
                throw new DomainException("Issuing country is required.");

            if (issueDate.HasValue && expiryDate.HasValue && issueDate >= expiryDate)
                throw new DomainException("Issue date must be before expiry date.");

            return new IdentityDocument(
                Guid.NewGuid(),                                 
                type,
                documentNumber,
                issuingAuthority,
                issuingCountry,
                issueDate,
                expiryDate,
                fileReference);
        }

        // ← needed for updating file after document is created
        public IdentityDocument WithFileReference(string? fileReference) =>
            new IdentityDocument(
                Id,                                             
                Type,
                DocumentNumber,
                IssuingAuthority,
                IssuingCountry,
                IssueDate,
                ExpiryDate,
                fileReference);

        public IdentityDocument WithExpiryDate(DateTime? expiryDate) =>
            new IdentityDocument(
                Id,                                             
                Type,
                DocumentNumber,
                IssuingAuthority,
                IssuingCountry,
                IssueDate,
                expiryDate,
                FileReference);

        public IdentityDocument WithId(Guid id) =>
            new IdentityDocument(
                id,
                Type,
                DocumentNumber,
                IssuingAuthority,
                IssuingCountry,
                IssueDate,
                ExpiryDate,
                FileReference);
    }
}
