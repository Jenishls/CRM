using System.Data;
using CRM.Application.Common.Errors;
using CRM.Domain.Customers.ValueObjects;
using ErrorOr;
using MediatR;

namespace CRM.Application.Customers.UpdateCustomer
{
    public class UpdateCustomerNameCommandHandler
        : IRequestHandler<UpdateCustomerNameCommand, ErrorOr<Unit>>
    {
        private readonly ICustomerRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCustomerNameCommandHandler(
            ICustomerRepository repository,
            IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<Unit>> Handle(
            UpdateCustomerNameCommand command,
            CancellationToken cancellationToken)
        {
            var customer = await _repository.GetByIdAsync(command.CifId, cancellationToken);
            if (customer is null) 
                return CustomerErrors.NotFound(command.CifId);
            if (!customer.IsActive) 
                return CustomerErrors.CustomerInactive(command.CifId);

            var fullName = FullName.Create(
                    command.FirstName,
                    command.MiddleName,
                    command.LastName);
            
            if(fullName == customer.FullName)
                return CustomerErrors.SameName();

            try
            {
                customer.UpdateName(fullName);
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