namespace CRM.Domain.Common
{
    // Base class for all entities in the domain
    public abstract class Entity<TId>
    {
        public TId Id { get; protected set; }
        protected Entity() { }

        protected Entity(TId id)
        {
            Id = id;
        }

        public override bool Equals(object? obj)
        {
            if (obj is not Entity<TId> other)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (GetType() != other.GetType())
                return false;

            return EqualityComparer<TId>.Default.Equals(Id, other.Id);
        }

        public override int GetHashCode()
            => HashCode.Combine(GetType(), Id);

        public static bool operator ==(Entity<TId>? a, Entity<TId>? b)
            => Equals(a, b);

        public static bool operator !=(Entity<TId>? a, Entity<TId>? b)
            => !Equals(a, b);
    }
}