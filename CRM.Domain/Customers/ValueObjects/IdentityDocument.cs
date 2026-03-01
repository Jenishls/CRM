using CMR.Domain.Enums;
using CRM.Domain.Common;

namespace CRM.Domain.ValueObjects
{
    public sealed class IdentityDocument : ValueObject
    {
        public DocumentType Type { get; }
        public string DocumentNumber { get; }
        public string IssuingCountry { get; }
        public DateTime IssueDate { get; }
        public DateTime? ExpiryDate { get; }
        public string? FileReference { get; private set; }   // e.g. "documents/12345/passport.pdf"

        private IdentityDocument() { } //EF

        public IdentityDocument(DocumentType type, string documentNumber, string issuingCountry, DateTime issueDate, DateTime? expiryDate = null, string? fileReference = null)
        {
            if (string.IsNullOrWhiteSpace(type.ToString())) throw new DomainException("Document type is required.");
            if (string.IsNullOrWhiteSpace(documentNumber)) throw new DomainException("Document number is required.");
            if (string.IsNullOrWhiteSpace(issuingCountry)) throw new DomainException("Issuing country is required.");

            Type = type;
            DocumentNumber = documentNumber.Trim();
            IssuingCountry = issuingCountry.Trim();
            IssueDate = issueDate;
            ExpiryDate = expiryDate;
            FileReference = fileReference?.Trim();
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Type;
            yield return DocumentNumber;
            yield return IssuingCountry;
            yield return IssueDate;
            yield return ExpiryDate;
        }
        // ...
    }
}