namespace CRM.Domain.Enums
{
    public enum KycStatus
    {
        NotStarted = 0,
        Inprogress = 1,
        PendingApproval      = 2,   // waiting for compliance / 2nd line review
        Approved             = 3,   // KYC/CDD/EDD passed → full services allowed
        Rejected             = 4,   // onboarding blocked / relationship refused
        Expired              = 5,   // periodic review overdue
        UnderReview          = 6,   // triggered review (event, risk change, transaction pattern…)
        EnhancedDueDiligence = 7,   // high-risk → EDD in progress
        Suspended            = 8,
    }
}