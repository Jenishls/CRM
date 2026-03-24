using ErrorOr;

namespace CRM.Application.Common.Errors;

public static class CustomerErrors
{
    //Not Found
    public static Error NotFound(int cifId) =>
        Error.NotFound(
            code: "Customer.NotFound",
            description: $"Customer with CIF ID '{cifId}' was not found.");
    
    public static Error NoCustomerFound =>
        Error.NotFound(
            code: "Customers.NotFound",
            description: "No customers found.");
        
    public static Error ContactNotFound(Guid id, int cifId) =>
        Error.NotFound(
            code: "Customer.ContactNotFound", 
            description: $"Contact with ID {id} was not found of CIF {cifId}.");

    public static Error AddressNotFound(Guid id, int cifId) =>
        Error.NotFound(
            code:"Customer.AddressNotFound",
            description: $"Address with ID {id} was not found of CIF {cifId}.");

    public static Error IdentificationNotFound(Guid id, int cifId) =>
        Error.NotFound(
            code:"Customer.IdentificationNotFound",
            description: $"Identification with ID {id} was not found of CIF {cifId}.");
    //Status
    public static Error CustomerInactive(int cifId) =>
        Error.Conflict(
            code: "Customer.Inactive",
            description: $"Customer with CIF {cifId} is inactive and cannot be updated.");

    // Primary
    public static Error PrimaryContactRequired() =>
        Error.Validation(
            code: "Customer.PrimaryContactRequired",
            description: "At least one primary contact must remain after update.");

    public static Error PrimaryAddressRequired() =>
        Error.Validation(
            code:"Customer.PrimaryAddressRequired", 
            description: "At least one primary address must remain after update.");
    
    // Minimum records
    public static Error MinimumContactRequired() =>
        Error.Validation(
            code:"Customer.MinimumContactRequired", 
            description: "Customer must have at least one contact.");

    public static Error MinimumAddressRequired() =>
        Error.Validation(
            code:"Customer.MinimumAddressRequired", 
            description: "Customer must have at least one address.");

    public static Error MinimumIdentificationRequired() =>
        Error.Validation(
            code:"Customer.MinimumIdentificationRequired",
            description: "Customer must have at least one identification document.");

    public static Error ContactExpired(Guid id) =>
        Error.Validation(
            code:"Customer.ContactExpired", 
            description: $"Contact has already expired.");

    public static Error AddressExpired(Guid id) =>
        Error.Validation(
            code:"Customer.AddressExpired", 
            description: $"Address with ID {id} has already expired.");

    public static Error IdentificationExpired(Guid id) =>
        Error.Validation(
            code:"Customer.IdentificationExpired",
            description: $"Identification with ID {id} has already expired.");
        // Concurrency
    public static Error ConcurrentUpdate() =>
        Error.Conflict(
            code:"Customer.ConcurrentUpdate", 
            description: "Customer was updated by another request. Please retry.");
    public static Error SameName() =>
        Error.Conflict(
            code:"Customer.SameNameUpdate", 
            description: "Customer was not updated, same name. Please retry.");

}