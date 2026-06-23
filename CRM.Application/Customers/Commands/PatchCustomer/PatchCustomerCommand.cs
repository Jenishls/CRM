using Crm.Application.Customers.Dtos.Update;
using CRM.Application.Customers.DTOs.Update;
using ErrorOr;
using MediatR;

namespace CRM.Application.Customers.Commands.PatchCustomer
{
    public class PatchCustomerCommand : IRequest<ErrorOr<Unit>>
    {
        public int CifId {get;set;}
        public PatchNameDto? Name {get; set;}
        public List<UpdateContactDto>? Contacts {get; set;}
        public List<UpdateAddressDto>? Addresses {get; set;}
        public List<UpdateIdentificationDto>? Identifications {get; set;}
        public PatchPersonalProfileDto? Profile {get; set;}
    }

    public record PatchNameDto(
        string FirstName,
        string? MiddleName,
        string LastName
    );

    public record PatchPersonalProfileDto(
        string? CustomerStatus,
        string? KycStatus,
        string? RiskLevel,
        string? RiskCategory,
        string? RiskSubCategory,
        decimal? AnnualIncome,
        string? SourceOfFunds,
        string? Occupation,
        string? EmployerName,
        string? EmploymentStatus,
        string? PurposeOfRelationship,
        decimal? ExpectedMonthlyTransactionVolume,
        DateTime? CustomerSince
    );

}
