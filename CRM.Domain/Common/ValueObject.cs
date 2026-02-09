namespace CRM.Domain.Common
{
    // Base class for all value objects in the domain
    public abstract class ValueObject
    {
        protected abstract IEnumerable<object?> GetEqualityComponents();

        public override bool Equals(object? obj)
            => obj is ValueObject vo && GetEqualityComponents().SequenceEqual(vo.GetEqualityComponents());

        public override int GetHashCode()
            => GetEqualityComponents().Aggregate(0, HashCode.Combine);

        public static bool operator ==(ValueObject? a, ValueObject? b) => Equals(a, b);
        public static bool operator !=(ValueObject? a, ValueObject? b) => !Equals(a, b);
    }
}