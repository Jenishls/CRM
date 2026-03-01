using System;
namespace CRM.Domain.Common
{
    // Base class for all domain exceptions
    public sealed class DomainException : Exception
    {
        public DomainException() { }

        public DomainException(string message) : base(message) { }
    }
}