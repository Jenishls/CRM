namespace CRM.Domain.Entities 
{
    public class Country
    {
        public Guid Id { get;  }
        public string Name { get; set; } = null!;
        public string CurrencyCode { get; set; } = null!; 
        public string Currency { get; set; } = null!;
        public string IsoCode { get; set; } = null!;

        private Country() { } // EF

        

    }
}