using MediatR;
using ErrorOr;
namespace CRM.Application.Customers.Commands.UpdateCustomerStatus
{
    public record UpdateCustomerStatusCommand(
        int CifId,
        bool IsActive
    ): IRequest<ErrorOr<Unit>>;
}