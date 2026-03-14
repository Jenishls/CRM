using CRM.Domain.Customers;
using Microsoft.EntityFrameworkCore;
public sealed class AppDbContext : DbContext, IUnitOfWork
{
    public DbSet<Customer> Customers => Set<Customer>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
    Task<int> IUnitOfWork.SaveChangesAsync(CancellationToken ct)
        => base.SaveChangesAsync(ct);
}