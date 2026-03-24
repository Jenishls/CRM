using Crm.Application.Customers.Dtos.Update;
using FluentValidation;

namespace CRM.Application.Customers.Validators.Update
{
    public class UpdateContactDtoValidator : ContactDtoValidator<UpdateContactDto>
    {
        public UpdateContactDtoValidator()
        {
            RuleFor(c => c.Id).NotEmpty().WithMessage("Contact Id is required");
        }
    } 
}