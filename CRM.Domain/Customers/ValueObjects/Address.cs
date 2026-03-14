using CRM.Domain.Common;
using CRM.Domain.Enums;

namespace CRM.Domain.Customers.ValueObjects
{
    public sealed class Address : ValueObject
    {
        public AddressType Type{get;}
        public DateTime ValidFrom { get; }
        public DateTime? ValidTo { get; private set; }
        public string Street { get; }
        public string City { get; }
        public string State { get; }
        public string ZipCode { get; }
        public string Country { get; }
        public bool IsPrimary { get; private set; }

        private Address() { } //EF

        private Address(
            AddressType type,
            DateTime validFrom,
            DateTime? validTo,
            string street,
            string city,
            string state,
            string zipCode,
            string country,
            bool isPrimary = false)
        {
            Type = type;
            ValidFrom = validFrom;
            ValidTo = validTo;
            Street = street ;
            City = city ;
            State = state;
            ZipCode = zipCode;
            Country = country;
            IsPrimary = isPrimary;

            
        }

        public static Address Create(AddressType type, string street, string city, string state, string zipCode, string country, bool isPrimary = false, DateTime? validFrom = null, DateTime? validTo = null)
        {
            if (type == 0) throw new DomainException("Address type is required.");
            if (string.IsNullOrWhiteSpace(street)) throw new DomainException("Street is required.");
            if (string.IsNullOrWhiteSpace(city)) throw new DomainException("City is required.");
            if (string.IsNullOrWhiteSpace(country)) throw new DomainException("Country is required.");
            if (validTo.HasValue && validTo < validFrom)
                throw new DomainException("ValidTo must be after ValidFrom");

            var addressType = Enum.TryParse<AddressType>(type.ToString(), true, out var parsedType) ? parsedType : AddressType.Residential; // Default to Home if parsing fails

            return new Address(addressType, validFrom ?? DateTime.UtcNow, validTo ?? DateTime.UtcNow.AddYears(100), street.Trim(), city.Trim(), state.Trim(), zipCode.Trim(), country.Trim(), isPrimary);
        }

        public Address WithIsPrimary(bool isPrimary) =>
            new Address(Type, ValidFrom, ValidTo, Street, City, State, ZipCode, Country, isPrimary);

        public Address WithValidTo(DateTime validTo) =>
            new Address(Type, ValidFrom, validTo, Street, City, State, ZipCode, Country, IsPrimary);


        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Type;
            yield return Street;
            yield return City;
            yield return State;
            yield return ZipCode;
            yield return Country;
        }
    }
}