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
        //public VerificationStatus VerificationStatus { get; private set; }

        private IdentityDocument() { } //EF

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