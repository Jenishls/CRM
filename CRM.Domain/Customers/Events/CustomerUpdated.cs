using CRM.Domain.Common;

namespace CRM.Domain.Customers.Events
{
    public sealed record CustomerUpdated(CustomerId CustomerId, DateTime OccurredOnUtc) : IDomainEvent;
}