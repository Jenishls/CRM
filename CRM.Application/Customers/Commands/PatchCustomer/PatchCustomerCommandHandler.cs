using System.Data;
using CRM.Application.Common.Errors;
using CRM.Application.Common.Utilities;
using CRM.Application.Customers.Services;
using CRM.Domain.Customers.ValueObjects;
using CRM.Domain.Enums;
using ErrorOr;
using MediatR;

namespace CRM.Application.Customers.Commands.PatchCustomer
{
    public class PatchCustomerCommandHandler : IRequestHandler<PatchCustomerCommand, ErrorOr<Unit>>
    {
        private readonly ICustomerRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly CustomerSyncService _syncService; 

        public PatchCustomerCommandHandler(
            ICustomerRepository repository,
            IUnitOfWork unitOfWork,
            CustomerSyncService customerSyncService
        )
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _syncService = customerSyncService;      
        }

        public async Task<ErrorOr<Unit>> Handle(PatchCustomerCommand command, CancellationToken cancellationToken)
        {
            var customer = await _repository.GetByIdAsync(command.CifId, cancellationToken);
            if(customer is null)
                return CustomerErrors.NotFound(command.CifId);
            if(!customer.IsActive)
                return CustomerErrors.CustomerInactive(command.CifId);
            
            if(command.Name is not null)
            {
                var fullName = FullName.Create(
                    command.Name.FirstName,
                    command.Name.MiddleName,
                    command.Name.LastName
                );
                customer.UpdateName(fullName);
            }

            if (command.Contacts is not null)
            {
                var result = _syncService.SyncContacts(customer, command.Contacts);
                if (result.IsError) return result.Errors;
            }

            if (command.Addresses is not null)
            {
                var result = _syncService.SyncAddresses(customer, command.Addresses);
                if (result.IsError) return result.Errors;
            }

            if (command.Identifications is not null)
            {
                var result = _syncService.SyncIdentifications(customer, command.Identifications);
                if (result.IsError) return result.Errors;
            }

            if (command.Profile is not null)
            {
                customer.UpdatePersonalProfile(
                    ParseOrDefault(command.Profile.CustomerStatus, customer.CustomerStatus),
                    ParseOrDefault(command.Profile.KycStatus, customer.KycStatus),
                    ParseOrDefault(command.Profile.RiskLevel, customer.RiskLevel),
                    command.Profile.RiskCategory ?? customer.RiskCategory,
                    command.Profile.RiskSubCategory ?? customer.RiskSubCategory,
                    command.Profile.AnnualIncome ?? customer.AnnualIncome,
                    command.Profile.SourceOfFunds ?? customer.SourceOfFunds,
                    command.Profile.Occupation ?? customer.Occupation,
                    command.Profile.EmployerName ?? customer.EmployerName,
                    command.Profile.EmploymentStatus ?? customer.EmploymentStatus,
                    command.Profile.PurposeOfRelationship ?? customer.PurposeOfRelationship,
                    command.Profile.ExpectedMonthlyTransactionVolume ?? customer.ExpectedMonthlyTransactionVolume,
                    command.Profile.CustomerSince ?? customer.CustomerSince);
            }

            try
            {
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
            catch (DBConcurrencyException)
            {
                return CustomerErrors.ConcurrentUpdate();
            }
            return Unit.Value;
        }

        private static TEnum ParseOrDefault<TEnum>(string? value, TEnum defaultValue)
            where TEnum : struct, Enum
            => string.IsNullOrWhiteSpace(value) ? defaultValue : EnumParser.Parse<TEnum>(value);
    }

}
