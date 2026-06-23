using CRM.Domain.Common;
using CRM.Domain.Enums;

namespace CRM.Domain.Customers.Entites
{
    public sealed class Contact
    {
        public Guid Id { get; private set; }
        public ContactType Type { get; private set; }
        public string Phone { get; private set; }
        public string? Email { get; private set; } 
        public bool IsPrimary { get; private set; }
        public DateTime? ValidFrom { get; private set; }
        public DateTime? ValidTo { get; private set; }
        private Contact() { } // EF

         private Contact(
            Guid id,
            ContactType type,
            string phone,
            string? email,
            bool isPrimary,
            DateTime? validFrom,
            DateTime? validTo)
        {
            Id = id;
            Type = type;
            Phone = phone.Trim();
            Email = email?.Trim();
            IsPrimary = isPrimary;
            ValidFrom = validFrom;
            ValidTo = validTo;
        }

        public static Contact Create(
            ContactType type,
            string phone,
            string? email,
            DateTime? validFrom = null,
            DateTime? validTo = null,
            bool isPrimary = false)
        {
            if (string.IsNullOrWhiteSpace(phone))
                throw new DomainException("Phone is required.");

            // Email only validated if provided
            if (email is not null && email.Trim().Length == 0)
                throw new DomainException("Email cannot be empty if provided.");

            return new Contact(
                Guid.NewGuid(),
                type,
                phone,
                email,
                isPrimary,
                validFrom,
                validTo);
        }

        public Contact WithIsPrimary(bool isPrimary) =>
            new Contact(Id, Type, Phone, Email, isPrimary, ValidFrom, ValidTo);

        public Contact WithValidTo(DateTime validTo) =>
            new Contact(Id, Type, Phone, Email, IsPrimary, ValidFrom, validTo);

        public Contact WithId(Guid id) =>
            new Contact(id, Type, Phone, Email, IsPrimary, ValidFrom, ValidTo);
    }
}
