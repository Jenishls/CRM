using CRM.Domain.Enums;
using CRM.Domain.ValueObjects;
using ErrorOr;

namespace CRM.Domain.Aggregates
{
    public sealed class Customer
    {
        private readonly List<KycReview> _kycReviews = [];

        private Customer() { } // EF

        private Customer(Guid id, FullName fullName, CustomerType customerType, KycProfile kycProfile)
        {
            Id = id;
            FullName = fullName;
            CustomerType = customerType;
            KycProfile = kycProfile;
            Status = CustomerStatus.Prospect;
            CreatedAtUtc = DateTime.UtcNow;
        }

        public Guid Id { get; }
        public FullName FullName { get; private set; } = null!;
        public CustomerType CustomerType { get; private set; }
        public CustomerStatus Status { get; private set; }
        public KycProfile KycProfile { get; private set; } = null!;
        public DateTime CreatedAtUtc { get; }

        public IReadOnlyCollection<KycReview> KycReviews => _kycReviews.AsReadOnly();

        public static ErrorOr<Customer> Create(FullName fullName, CustomerType customerType)
        {
            if (fullName is null)
            {
                return Error.Validation(
                    code: "Customer.FullNameRequired",
                    description: "Customer full name is required.");
            }

            var initialKyc = KycProfile.Create(
                status: KycStatus.NotStarted,
                riskLevel: RiskLevel.Unknown);

            if (initialKyc.IsError)
            {
                return initialKyc.Errors;
            }

            return new Customer(Guid.NewGuid(), fullName, customerType, initialKyc.Value);
        }

        public ErrorOr<Success> UpdateFullName(FullName fullName)
        {
            if (fullName is null)
            {
                return Error.Validation(
                    code: "Customer.FullNameRequired",
                    description: "Customer full name is required.");
            }

            FullName = fullName;
            return Result.Success;
        }

        public ErrorOr<Success> AddKycReview(
            KycStatus status,
            RiskLevel riskLevel,
            string reviewedBy,
            string? notes = null,
            DateTime? nextReviewDueAtUtc = null)
        {
            var reviewResult = KycReview.Create(status, riskLevel, reviewedBy, notes);
            if (reviewResult.IsError)
            {
                return reviewResult.Errors;
            }

            var profileResult = KycProfile.WithStatus(status, riskLevel, nextReviewDueAtUtc);
            if (profileResult.IsError)
            {
                return profileResult.Errors;
            }

            KycProfile = profileResult.Value;
            _kycReviews.Add(reviewResult.Value);

            return Result.Success;
        }

        public void ChangeStatus(CustomerStatus status)
        {
            Status = status;
        }
    }
}
