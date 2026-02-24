using CRM.Domain.Common;
using CRM.Domain.Enums;
using ErrorOr;

namespace CRM.Domain.ValueObjects
{
    public sealed class KycProfile : ValueObject
    {
        public KycStatus Status { get; }
        public RiskLevel RiskLevel { get; }
        public DateTime LastUpdatedAtUtc { get; }
        public DateTime? NextReviewDueAtUtc { get; }

        private KycProfile() { } // EF

        private KycProfile(
            KycStatus status,
            RiskLevel riskLevel,
            DateTime lastUpdatedAtUtc,
            DateTime? nextReviewDueAtUtc)
        {
            Status = status;
            RiskLevel = riskLevel;
            LastUpdatedAtUtc = lastUpdatedAtUtc;
            NextReviewDueAtUtc = nextReviewDueAtUtc;
        }

        public static ErrorOr<KycProfile> Create(
            KycStatus status,
            RiskLevel riskLevel,
            DateTime? nextReviewDueAtUtc = null,
            DateTime? lastUpdatedAtUtc = null)
        {
            var errors = new List<Error>();
            var effectiveUpdatedAt = lastUpdatedAtUtc ?? DateTime.UtcNow;

            if (nextReviewDueAtUtc is not null && nextReviewDueAtUtc <= effectiveUpdatedAt)
            {
                errors.Add(Error.Validation(
                    code: "KycProfile.NextReviewDueAtInvalid",
                    description: "Next KYC review date must be in the future."));
            }

            if (errors.Count > 0)
            {
                return errors;
            }

            return new KycProfile(status, riskLevel, effectiveUpdatedAt, nextReviewDueAtUtc);
        }

        public ErrorOr<KycProfile> WithStatus(KycStatus status, RiskLevel riskLevel, DateTime? nextReviewDueAtUtc = null)
            => Create(status, riskLevel, nextReviewDueAtUtc, DateTime.UtcNow);

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Status;
            yield return RiskLevel;
            yield return LastUpdatedAtUtc;
            yield return NextReviewDueAtUtc;
        }
    }
}
