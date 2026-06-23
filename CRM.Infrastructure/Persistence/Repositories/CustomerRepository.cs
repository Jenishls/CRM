using CRM.Domain.Customers;
using CRM.Domain.Enums;
using Microsoft.EntityFrameworkCore;

public sealed class CustomerRepository : ICustomerRepository
{
    private readonly AppDbContext _db;

    public CustomerRepository(AppDbContext db)
    {
        _db = db;
    }

    public Task AddAsync(Customer customer, CancellationToken ct = default)
        => _db.Customers.AddAsync(customer, ct).AsTask();

    public Task<Customer?> GetByIdAsync(int cifId, CancellationToken ct = default)
        => _db.Customers
            .Include(c => c.Contacts)
            .Include(c => c.Addresses)
            .Include(c => c.IdentityDocuments)
            .FirstOrDefaultAsync(x => x.CifId == cifId, ct);

    public async Task<List<Customer>>  GetAllAsync(CancellationToken ct = default)
        => await _db.Customers
            .Include(c => c.Contacts)
            .Include(c => c.Addresses)
            .Include(c => c.IdentityDocuments)
            .AsNoTracking()
            .ToListAsync(ct);

    public void Update(Customer customer)
        => _db.Customers.Update(customer);

    public Task<bool> ExistsByEmailAsync(string email, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(email))
            return Task.FromResult(false);

        email = email.Trim().ToLowerInvariant();

        return _db.Customers.AnyAsync(
            c => c.Contacts.Any(x => x.Email != null && x.Email.ToLower() == email),
            ct);
    }

    public Task<bool> ExistsByIdentificationNumbersAsync(
        IEnumerable<string> identificationNumbers,
        CancellationToken ct = default)
    {
        var numbers = identificationNumbers?
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => x.Trim())
            .Distinct()
            .ToList() ?? new List<string>();

        if (numbers.Count == 0)
            return Task.FromResult(false);

        return _db.Customers.AnyAsync(
            c => c.IdentityDocuments.Any(d => numbers.Contains(d.DocumentNumber)),
            ct);
    }

    public Task<bool> ExistsByNationalIdAsync(string nationalId, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(nationalId))
            return Task.FromResult(false);

        nationalId = nationalId.Trim();

        return _db.Customers.AnyAsync(
            c => c.IdentityDocuments.Any(d =>
                d.Type == DocumentType.NationalID &&
                d.DocumentNumber == nationalId),
            ct);
    }

}
