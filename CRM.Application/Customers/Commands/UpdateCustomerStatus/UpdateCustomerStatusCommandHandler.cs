using CRM.Application.Common.Errors;
using ErrorOr;
using MediatR;

namespace CRM.Application.Customers.Commands.UpdateCustomerStatus
{
    public class UpdateCustomterStatusCommandHandler :  IRequestHandler<UpdateCustomerStatusCommand, ErrorOr<Unit>>
    {
        private readonly ICustomerRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCustomterStatusCommandHandler(
            ICustomerRepository repository,
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public async Task<ErrorOr<Unit>> Handle(UpdateCustomerStatusCommand request, CancellationToken cancellationToken)
        {
            var customer = await _repository.GetByIdAsync(request.CifId, cancellationToken);
            if(customer is null )
            {
                return CustomerErrors.NotFound(request.CifId);
            }

            if (request.IsActive)
            {
                customer.Activate();
            }
            else
            {
                customer.Deactivate();
            }

            // _repository.Update(customer);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}