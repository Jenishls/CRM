namespace CRM.Domain.Enums

public enum CustomerStatus
{
    Prospect         = 0,   // lead / application started, no contract yet
    Active           = 1,   // normal – can use products & services
    Dormant          = 2,   // no activity for X months/years (triggers reactivation checks)
    Restricted       = 3,   // limited operations (e.g. outgoing transfers blocked)
    Frozen           = 4,   // strong suspicion / court order / sanctions hit
    Closed           = 5,   // voluntarily / end of relationship
    Deleted          = 6,   // soft-deleted (your requirement – audit trail preserved)
    Blocked       = 7,   // 
    Blacklisted   = 8    // very strong – usually separate watch-list table
}