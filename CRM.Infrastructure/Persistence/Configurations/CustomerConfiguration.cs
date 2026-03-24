using CRM.Domain.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");

        builder.HasKey(c => c.Id);

        builder.Property(c=> c.RowVersion).IsRowVersion().IsRequired();
        
        builder.Property(c => c.Id)
            .HasColumnName("Id")
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => new CustomerId(value));

        builder.OwnsOne(c => c.FullName, fullName =>
        {
        fullName.Property(n => n.First)
            .HasColumnName("FirstName")
            .HasMaxLength(100)
            .IsRequired();

        fullName.Property(n => n.Middle)
            .HasColumnName("MiddleName")
            .HasMaxLength(100);

        fullName.Property(n => n.Last)
            .HasColumnName("LastName")
            .HasMaxLength(100)
            .IsRequired();
    });
        builder.Property(c => c.CifId)
       .ValueGeneratedOnAdd();
       
        builder.Property(c => c.IsActive)
            .IsRequired();

        builder.OwnsMany(c => c.Contacts, contact =>
        {
            contact.ToTable("CustomerContacts");
            contact.WithOwner().HasForeignKey("CustomerId");

            // contact.Property<Guid>("Id");
            contact.HasKey("Id");
            contact.Property(c => c.Id).ValueGeneratedNever();

            contact.Property(c => c.Type).HasConversion<string>().HasMaxLength(50).IsRequired();
            contact.Property(c => c.Phone).HasMaxLength(20);
            contact.Property(c => c.Email).HasMaxLength(256);
            contact.Property(c => c.ValidFrom);
            contact.Property(c => c.ValidTo);
            contact.Property(c => c.IsPrimary).IsRequired();
        });

        builder.OwnsMany(c => c.Addresses, address =>
        {
            address.ToTable("CustomerAddresses");
            address.WithOwner().HasForeignKey("CustomerId");

            // address.Property<Guid>("Id");
            address.HasKey("Id");
            address.Property(a => a.Id).ValueGeneratedNever(); 

            address.Property(a => a.Type).HasConversion<string>().HasMaxLength(50).IsRequired();
            address.Property(a => a.Street).HasMaxLength(200).IsRequired();
            address.Property(a => a.City).HasMaxLength(100).IsRequired();
            address.Property(a => a.State).HasMaxLength(100);
            address.Property(a => a.ZipCode).HasMaxLength(20);
            address.Property(a => a.Country).HasMaxLength(100).IsRequired();
            address.Property(a => a.ValidFrom);
            address.Property(a => a.ValidTo);
            address.Property(a => a.IsPrimary).IsRequired();
        });

        builder.OwnsMany(c => c.IdentityDocuments, doc =>
        {
            doc.ToTable("CustomerIdentityDocuments");

            doc.WithOwner().HasForeignKey("CustomerId");

            // doc.Property<Guid>("Id");
            doc.HasKey("Id");
            doc.Property(d => d.Id).ValueGeneratedNever(); 

            doc.Property(d => d.Type)
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();

            doc.Property(d => d.DocumentNumber)
                .HasMaxLength(100)
                .IsRequired();

            doc.Property(d => d.IssuingAuthority)
                .HasMaxLength(100)
                .IsRequired();

            doc.Property(d => d.IssuingCountry)
                .HasMaxLength(100)
                .IsRequired();

            doc.Property(d => d.IssueDate)
                .IsRequired();

            doc.Property(d => d.ExpiryDate);

            // only if your domain actually has this property
            // doc.Property(d => d.IsPrimary).IsRequired();
        });

        builder.Navigation(c => c.Contacts).UsePropertyAccessMode(PropertyAccessMode.Field);
        builder.Navigation(c => c.Addresses).UsePropertyAccessMode(PropertyAccessMode.Field);
        builder.Navigation(c => c.IdentityDocuments).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}