using CRM.Application.Customers.DTOs;
using CRM.Domain.Enums;
using FluentValidation;

namespace CRM.Application.Customers.Validators
{
    public class IdentificationDtoValidator<T> : AbstractValidator<T>
    where T : IdentificationDto
    {
        public IdentificationDtoValidator()
        {
            RuleFor(i => i.Type).NotEmpty().Must(type => IsValidDocumentType(type)).WithMessage("Invalid document type.");
            RuleFor(i => i.Type).NotEmpty().WithMessage("Identification type is required.");
            RuleFor(i => i.DocumentNumber).NotEmpty().WithMessage("Identification number is required.");
            RuleFor(i => i.IssuingAuthority).NotEmpty().WithMessage("Identification issuing authority is required.");
            RuleFor(i => i.IssuedDate).LessThan(i => i.ExpiryDate).When(i => i.ExpiryDate.HasValue).WithMessage("Issued date must be before expiry date.");
        }

        private static bool IsValidDocumentType(string value) =>
            Enum.TryParse<DocumentType>(value, out _);
    
    }

    public class IdentificationDtoValidator : IdentificationDtoValidator<IdentificationDto> { }

}