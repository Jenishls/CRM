namespace CRM.Domain.Customers
{
    public readonly record struct CustomerId(Guid Value)
    {
        public static CustomerId New() => new(Guid.NewGuid());
        public static CustomerId Create(Guid value) => new(value);
        public override string ToString() => Value.ToString();
    }
}