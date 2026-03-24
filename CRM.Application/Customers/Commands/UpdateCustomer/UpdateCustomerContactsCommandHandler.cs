using System.Data;
using CRM.Application.Common.Errors;
using CRM.Application.Customers.Services;
using CRM.Application.Customers.UpdateCustomer;
using ErrorOr;
using MediatR;

namespace CRM.Application.Customers.Commands
{
    public class UpdateCustomerContactsCommandHandler : IRequestHandler<UpdateCustomerContactsCommand, ErrorOr<Unit>>
    {
        private readonly ICustomerRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly CustomerSyncService _syncService;

        public UpdateCustomerContactsCommandHandler(
            ICustomerRepository repository,
            IUnitOfWork unitOfWork,
            CustomerSyncService syncService)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _syncService = syncService;
        }

        public async Task<ErrorOr<Unit>> Handle(
            UpdateCustomerContactsCommand command,
            CancellationToken cancellationToken)
        {
            var customer = await _repository.GetByIdAsync(command.CifId, cancellationToken);
            if (customer is null) 
                return CustomerErrors.NotFound(command.CifId);
            if (!customer.IsActive) 
                return CustomerErrors.CustomerInactive(command.CifId);

            var result = _syncService.SyncContacts(customer, command.Contacts);
            if (result.IsError) 
                return result.Errors;

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
    }

}