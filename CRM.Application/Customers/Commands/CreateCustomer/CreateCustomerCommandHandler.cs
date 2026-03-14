using CRM.Domain.Enums;
using CRM.Domain.Customers;
using CRM.Domain.Customers.ValueObjects;
using ErrorOr;
using MediatR;
using CRM.Application.Customers.Commands.CreateCustomer;
using CRM.Application.Common.Utilities;


public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, ErrorOr<Guid>>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;
    public CreateCustomerCommandHandler(ICustomerRepository customerRepository, IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<Guid>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var primaryEmail = request.Contacts.FirstOrDefault(c => c.IsPrimary)?.Email.Trim().ToLowerInvariant();
        var nationlId = request.Identifications.FirstOrDefault(i => i.Type.ToString() == DocumentType.NationalID.ToString())?.DocumentNumber.Trim();
        var identificationNumbers = request.Identifications.Select(i => i.DocumentNumber.Trim()).ToList();

        if (await _customerRepository.ExistsByEmailAsync(primaryEmail, cancellationToken))
            return Error.Conflict("A customer with the same primary email already exists.");
        
        if(await _customerRepository.ExistsByIdentificationNumbersAsync(identificationNumbers, cancellationToken))
            return Error.Conflict("A customer with the same identification number already exists.");

        if(await _customerRepository.ExistsByNationalIdAsync(nationlId, cancellationToken))
            return Error.Conflict("A customer with the same national ID already exists.");
        
        var contacts = request.Contacts
            .Select(c => Contact.Create(
                
                EnumParser.Parse<ContactType>(c.Type),
                c.Phone,
                c.Email,
                c.ValidFrom,
                c.ValidTo,
                c.IsPrimary))
                .ToList();

        var addresses = request.Addresses
            .Select(a => Address.Create(
                EnumParser.Parse<AddressType>(a.Type),
                a.Street.Trim(),
                a.City.Trim(),
                a.State.Trim(),
                a.ZipCode,
                a.Country.Trim(),
                a.IsPrimary,
                a.ValidFrom,
                a.ValidTo))
            .ToList();

        var identifications = request.Identifications
            .Select(i => IdentityDocument.Create(
                EnumParser.Parse<DocumentType>(i.Type),
                i.DocumentNumber.Trim(),
                i.IssuingAuthority.Trim(),
                i.IssuingCountry.Trim(),
                i.IssuedDate ?? DateTime.UtcNow,
                i.ExpiryDate))
            .ToList();

        var customer = Customer.CreateNew(
            FullName.Create(request.FirstName.Trim(), request.MiddleName?.Trim(), request.LastName.Trim()),
            contacts ,
            addresses,
            identifications);

        try
        {
            await _customerRepository.AddAsync(customer, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }catch  (Exception ex)
        {
            return Error.Failure("An error occurred while creating the customer: " + ex.Message);
        }

        return customer.Id.Value;
    }

}