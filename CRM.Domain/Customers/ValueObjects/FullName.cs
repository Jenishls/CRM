using System;
using CRM.Domain.Common;

namespace CRM.Domain.Customers.ValueObjects
{
    public sealed class FullName : ValueObject
    {
        public  string First { get; private set;}
        public string? Middle { get; private set;}
        public string Last { get; private set;}

        private FullName() { } // EF
    
        private FullName(string firstName, string? middleName, string lastName)
        {
            First = firstName;
            Middle = middleName;
            Last = lastName;
        }

        public static FullName Create(string first,  string? middle = null, string? last = null)
        {
            if (string.IsNullOrWhiteSpace(first)) throw new DomainException("First name is required.");
            if (string.IsNullOrWhiteSpace(last))  throw new DomainException("Last name is required.");

             return new FullName(first.Trim(), middle?.Trim(), last.Trim());
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