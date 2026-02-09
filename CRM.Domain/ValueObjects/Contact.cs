using CRM.Domain.Common;
using CRM.Domain.Enums;

namespace Crm.Domain.ValueObjects
{
    public sealed class ContactDetail : ValueObject
    {
        public ContactType Type { get; }           // Email, PhoneMobile, PhoneWork, etc.
        public string ContactValue { get; }
        public DateTime ValidFrom { get; }
        public DateTime? ValidTo { get; private set; }
        public bool IsPrimary { get; private set; }

        public DateTime CreatedAt { get; }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Type;
            yield return ContactValue;
            yield return ValidFrom;
            yield return ValidTo;

        }
    }
}

