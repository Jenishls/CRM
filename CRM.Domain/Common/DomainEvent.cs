namespace CRM.Domain.Common
{
    public abstract record DomainEvent(DateTime OccurredOnUtc) : IDomainEvent;
    
}