using CRM.Domain.Enums;
using CRM.Domain.Common;

namespace CRM.Domain.Entities
{
    public sealed class Address
    {
        public Guid Id { get; private set; }
        public AddressType Type { get; private set; }
        public string Street { get; private set; }
        public string City { get; private set; }
        public string State { get; private set; }
        public string ZipCode { get; private set; }
        public string Country { get; private set; }
        public bool IsPrimary { get; private set; }
        public DateTime? ValidFrom { get; private set; }
        public DateTime? ValidTo { get; private set; }

        private Address() { }

        private Address(
            Guid id,
            AddressType type,
            string street,
            string city,
            string state,
            string zipCode,
            string country,
            bool isPrimary,
            DateTime? validFrom,
            DateTime? validTo)
        {
            Id = id;
            Type = type;
            Street = street.Trim();
            City = city.Trim();
            State = state.Trim();
            ZipCode = zipCode.Trim();
            Country = country.Trim();
            IsPrimary = isPrimary;
            ValidFrom = validFrom;
            ValidTo = validTo;
        }

        public static Address Create(
            AddressType type,
            string street,
            string city,
            string state,
            string zipCode,
            string country,
            bool isPrimary = false,
            DateTime? validFrom = null,
            DateTime? validTo = null)
        {
            // ← removed redundant enum check
            if (string.IsNullOrWhiteSpace(street))
                throw new DomainException("Street is required.");

            if (string.IsNullOrWhiteSpace(city))
                throw new DomainException("City is required.");

            if (string.IsNullOrWhiteSpace(state))
                throw new DomainException("State is required.");

            if (string.IsNullOrWhiteSpace(zipCode))
                throw new DomainException("Zip code is required.");

            if (string.IsNullOrWhiteSpace(country))
                throw new DomainException("Country is required.");

            if (validFrom.HasValue && validTo.HasValue && validFrom >= validTo)
                throw new DomainException("ValidFrom must be before ValidTo.");

            return new Address(
                Guid.NewGuid(),
                type,
                street,
                city,
                state,
                zipCode,
                country,
                isPrimary,
                validFrom,
                validTo);
        }

        public Address WithIsPrimary(bool isPrimary) =>
            new Address(
                Id,
                Type,
                Street,
                City,
                State,
                ZipCode,
                Country,
                isPrimary,
                ValidFrom,
                ValidTo);

        public Address WithId(Guid id) =>
            new Address(
                id,
                Type,
                Street,
                City,
                State,
                ZipCode,
                Country,
                IsPrimary,
                ValidFrom,
                ValidTo);
    }
}
