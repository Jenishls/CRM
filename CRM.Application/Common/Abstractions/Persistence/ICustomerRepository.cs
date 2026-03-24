using CRM.Domain.Customers;

public interface ICustomerRepository
{
    Task AddAsync(Customer customer, CancellationToken ct);
    Task<Customer?> GetByIdAsync(int cifId, CancellationToken ct = default);
    Task<List<Customer>> GetAllAsync(CancellationToken ct = default);

    Task<bool> ExistsByEmailAsync(string email, CancellationToken ct = default);
    Task<bool> ExistsByIdentificationNumbersAsync(IEnumerable<string> identificationNumbers, CancellationToken ct = default);
    Task<bool> ExistsByNationalIdAsync(string nationalId, CancellationToken ct = default);

    void Update(Customer customer);
    //void Remove(Customer customer);
}