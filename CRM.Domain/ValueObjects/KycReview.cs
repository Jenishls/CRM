using CRM.Domain.Common;
using CRM.Domain.Enums;
using ErrorOr;

namespace CRM.Domain.ValueObjects
{
    public sealed class KycReview : ValueObject
    {
        public KycStatus Status { get; }
        public RiskLevel RiskLevel { get; }
        public DateTime ReviewedAtUtc { get; }
        public string ReviewedBy { get; }
        public string? Notes { get; }

        private KycReview() { } // EF

        private KycReview(
            KycStatus status,
            RiskLevel riskLevel,
            DateTime reviewedAtUtc,
            string reviewedBy,
            string? notes)
        {
            Status = status;
            RiskLevel = riskLevel;
            ReviewedAtUtc = reviewedAtUtc;
            ReviewedBy = reviewedBy;
            Notes = notes;
        }

        public static ErrorOr<KycReview> Create(
            KycStatus status,
            RiskLevel riskLevel,
            string reviewedBy,
            string? notes = null,
            DateTime? reviewedAtUtc = null)
        {
            var errors = new List<Error>();

            if (string.IsNullOrWhiteSpace(reviewedBy))
            {
                errors.Add(Error.Validation(
                    code: "KycReview.ReviewedByRequired",
                    description: "Reviewer information is required."));
            }

            if (notes is { Length: > 1000 })
            {
                errors.Add(Error.Validation(
                    code: "KycReview.NotesLength",
                    description: "Review notes cannot exceed 1000 characters."));
            }

            if (errors.Count > 0)
            {
                return errors;
            }

            return new KycReview(
                status,
                riskLevel,
                reviewedAtUtc ?? DateTime.UtcNow,
                reviewedBy.Trim(),
                notes?.Trim());
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Status;
            yield return RiskLevel;
            yield return ReviewedAtUtc;
            yield return ReviewedBy;
            yield return Notes;
        }
    }
}
