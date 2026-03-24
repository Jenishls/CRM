using Crm.Application.Customers.Dtos.Update;
using FluentValidation;

namespace CRM.Application.Customers.Validators.Update
{
    public class UpdateIdentificationDtoValidator : IdentificationDtoValidator<UpdateIdentificationDto>
    {
        public UpdateIdentificationDtoValidator()
        {
            RuleFor(c => c.Id).NotEmpty().WithMessage("Document Id is required");
        }
    } 
}