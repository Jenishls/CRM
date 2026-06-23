using MediatR;
using ErrorOr;
using CRM.Application.Customers.DTOs;
using CRM.Application.Customers.Commands.Common;

namespace CRM.Application.Customers.Commands.CreateCustomer
{
    public class CreateCustomerCommand : ICustomerCommand, IRequest<ErrorOr<Guid>>
    {
        public string FirstName { get; set; } = default!;
        public string? MiddleName { get; set; }
        public string LastName { get; set; } = default!;
        public List<ContactDto> Contacts { get; set; } = new();
        public List<AddressDto> Addresses { get; set; } = new();
        public List<IdentificationDto> Identifications { get; set; } = new();
        public string? CustomerStatus { get; set; }
        public string? KycStatus { get; set; }
        public string? RiskLevel { get; set; }
        public string? RiskCategory { get; set; }
        public string? RiskSubCategory { get; set; }
        public decimal? AnnualIncome { get; set; }
        public string? SourceOfFunds { get; set; }
        public string? Occupation { get; set; }
        public string? EmployerName { get; set; }
        public string? EmploymentStatus { get; set; }
        public string? PurposeOfRelationship { get; set; }
        public decimal? ExpectedMonthlyTransactionVolume { get; set; }
        public DateTime? CustomerSince { get; set; }
    }
}
