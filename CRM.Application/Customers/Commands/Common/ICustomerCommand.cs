namespace CRM.Application.Customers.Commands.Common;

public interface ICustomerCommand
{
    string FirstName { get; }
    string? MiddleName {get;}
    string LastName { get; }
}