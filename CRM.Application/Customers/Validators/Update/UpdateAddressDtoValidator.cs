using CRM.Application.Customers.DTOs.Update;
using FluentValidation;

namespace CRM.Application.Customers.Validators.Update
{
    public class UpdateAddressDtoValidator : AddressDtoValidator<UpdateAddressDto>
    {
        public UpdateAddressDtoValidator()
        {
            RuleFor(a => a.Id).NotEmpty().WithMessage("Address Id is required for update");
        }
    }
}