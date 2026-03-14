namespace CRM.Domain.Customers
{
    public readonly record struct CustomerId(Guid Value)
    {
        public static CustomerId New() => new(Guid.NewGuid());
            // public static CustomerId New(long sequence)
                // => new($"CUST-{sequence:D6}");

        public override string ToString() => Value.ToString();
    }
}