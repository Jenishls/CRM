using CRM.Domain.Common;
using CRM.Domain.Enums;

namespace CRM.Domain.ValueObjects
{
    public class Address : ValueObject
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

           public Address(
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
        Street = street ?? throw new DomainException(nameof(street));
        City = city ?? throw new DomainException(nameof(city));
        State = state;
        ZipCode = zipCode;
        Country = country ?? throw new DomainException(nameof(country));
        IsPrimary = isPrimary;

        if (validTo.HasValue && validTo < validFrom)
            throw new DomainException("ValidTo must be after ValidFrom");
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