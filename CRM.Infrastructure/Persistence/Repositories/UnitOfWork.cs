namespace CRM.Infrastructure.Persistence.Repositories;
public sealed class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    async Task<int> IUnitOfWork.SaveChangesAsync(CancellationToken ct )
    {
        return await _context.SaveChangesAsync(ct);
    }
}