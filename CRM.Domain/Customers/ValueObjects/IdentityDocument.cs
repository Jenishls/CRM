using CRM.Domain.Enums;
using CRM.Domain.Common;

namespace CRM.Domain.Customers.ValueObjects
{
    public sealed class IdentityDocument : ValueObject
    {
        public DocumentType Type { get; private set; }
        public string DocumentNumber { get; private set; }
        public string IssuingAuthority { get; private set; }
        public string IssuingCountry { get; private set; }
        public DateTime IssueDate { get; private set; }
        public DateTime? ExpiryDate { get; private set; }
        public string? FileReference { get; private set; }   // e.g. "documents/12345/passport.pdf"

        private IdentityDocument() { } //EF

        private IdentityDocument(DocumentType type, string documentNumber, string issuingAuthority,string issuingCountry, DateTime issueDate, DateTime? expiryDate = null, string? fileReference = null)
        {
            Type = type;
            DocumentNumber = documentNumber.Trim();
            IssuingAuthority = issuingAuthority.Trim();
            IssuingCountry = issuingCountry.Trim();
            IssueDate = issueDate;
            ExpiryDate = expiryDate;
            FileReference = fileReference?.Trim();
        } 
        public static IdentityDocument Create(DocumentType type, string documentNumber, string issuingAuthority, string issuingCountry, DateTime issueDate, DateTime? expiryDate = null, string? fileReference = null)
        {
            if (string.IsNullOrWhiteSpace(type.ToString())) throw new DomainException("Document type is required.");
            if (string.IsNullOrWhiteSpace(documentNumber)) throw new DomainException("Document number is required.");
            if (string.IsNullOrWhiteSpace(issuingAuthority)) throw new DomainException("Issuing authority is required.");
            if (string.IsNullOrWhiteSpace(issuingCountry)) throw new DomainException("Issuing country is required.");

            return new IdentityDocument(type, documentNumber, issuingAuthority, issuingCountry, issueDate, expiryDate, fileReference);
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