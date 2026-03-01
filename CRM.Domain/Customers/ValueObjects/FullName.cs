using System;
using CRM.Domain.Common;

namespace CRM.Domain.Customers.ValueObjects
{
    public sealed class FullName : ValueObject
    {
        public string First { get; }
        public string? Middle { get; }
        public string Last { get; }

        public FullName(string first, string last, string? middle = null)
        {
            if (string.IsNullOrWhiteSpace(first)) throw new DomainException("First name is required.");
            if (string.IsNullOrWhiteSpace(last))  throw new DomainException("Last name is required.");

            First = first.Trim();
            Middle = string.IsNullOrWhiteSpace(middle) ? null : middle.Trim();
            Last  = last.Trim();
        }
 
        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return First;
            yield return Middle;
            yield return Last;
        }

        public override string ToString()
            => Middle is null ? $"{First} {Last}" : $"{First} {Middle} {Last}";
    }
}