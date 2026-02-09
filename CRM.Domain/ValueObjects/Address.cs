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

        public Address(string street, string city, string state, string zipCode, string country)
        {
            Street = street;
            City = city;
            State = state;
            ZipCode = zipCode;
            Country = country;
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Street;
            yield return City;
            yield return State;
            yield return ZipCode;
            yield return Country;
        }
    }
}