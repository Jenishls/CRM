using CRM.Domain.Common;
using CRM.Domain.Enums;

namespace CRM.Domain.Customers.ValueObjects
{
    public sealed class Contact: ValueObject
    {
        public ContactType Type { get; }           //  Mobile, Work,Home etc.
        public string Phone { get; }
        public string Email { get; }
        public DateTime ValidFrom { get; }
        public DateTime? ValidTo { get; private set; }
        public bool IsPrimary { get; private set; }
        private Contact() { } // EF

        private Contact(string type, string phone, string email, bool isPrimary, DateTime? validFrom = null, DateTime? validTo = null)
        {
            Type = Enum.TryParse<ContactType>(type, true, out var parsedType) ? parsedType : ContactType.Mobile; // Default to Mobile if parsing fails
            Phone = phone.Trim();
            Email = email.Trim();
            ValidFrom = validFrom ?? DateTime.UtcNow;
            ValidTo = validTo ?? DateTime.UtcNow.AddYears(100); // Default to a far future date if not provided
            IsPrimary = isPrimary;
        }
        public static Contact Create(ContactType type, string phone, string email, DateTime? validFrom=null,DateTime? validTo=null, bool isPrimary = false)
        {
            if (string.IsNullOrWhiteSpace(type.ToString())) throw new DomainException("Contact type is required.");
            if (string.IsNullOrWhiteSpace(phone)) throw new DomainException("Phone is required.");
            if (string.IsNullOrWhiteSpace(email)) throw new DomainException("Email is required.");

            return new Contact(type.ToString(), phone, email, isPrimary, validFrom, validTo);
        }

        public Contact WithValidTo(DateTime validTo)=>
            new Contact(Type.ToString(), Phone, Email, IsPrimary, validTo, ValidFrom);
        
        public Contact WithIsPrimary(bool isPrimary) =>
            new Contact(Type.ToString(), Phone, Email, isPrimary, ValidFrom, ValidTo);

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Type;
            yield return Phone;
            yield return Email;
            yield return ValidFrom;
            yield return ValidTo;

        }
    }
}

