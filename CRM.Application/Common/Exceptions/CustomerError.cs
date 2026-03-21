using ErrorOr;

namespace CRM.Application.Common.Errors;

public static class CustomerErrors
{
    public static Error NotFound(int cifId) =>
        Error.NotFound(
            code: "Customer.NotFound",
            description: $"Customer with CIF ID '{cifId}' was not found.");
    
        public static Error NoCustomerFound =>
        Error.NotFound(
            code: "Customers.NotFound",
            description: "No customers found.");
}