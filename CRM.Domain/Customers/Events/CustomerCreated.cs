using CRM.Domain.Common;

namespace CRM.Domain.Customers.Events
{
    public sealed record CustomerCreated(CustomerId CustomerId, DateTime OccurredOnUtc) : IDomainEvent;
}