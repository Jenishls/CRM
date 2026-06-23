using CRM.Application.Common.Errors;
using CRM.Application.Customers.Queries.GetCustomerByCif;
using CRM.Contracts.Responses;
using ErrorOr;
using MediatR;

namespace Crm.Application.Customers.Queries.GetCustomerByCif;

public class GetCustomerByCifHandler : IRequestHandler<GetCustomerByCifQuery, ErrorOr<CustomerDetailResponse?>>
{
    private readonly ICustomerRepository _repository;

    public GetCustomerByCifHandler(ICustomerRepository repository)
    {
        _repository = repository;
    }

    public async Task<ErrorOr<CustomerDetailResponse?>> Handle(GetCustomerByCifQuery request, CancellationToken ct)
    {
        var response = await _repository.GetByIdAsync(request.CifId, ct);
        if (response is null)
        {
          return CustomerErrors.NotFound(request.CifId);
        }

        var customer = new CustomerDetailResponse
            {
                Id = response.Id.Value,
                CifId = response.CifId,
                FirstName = response.FullName.First,
                MiddleName = response.FullName.Middle,
                LastName = response.FullName.Last,
                IsActive = response.IsActive,
                CustomerType = response.CustomerType.ToString(),
                CustomerStatus = response.CustomerStatus.ToString(),
                KycStatus = response.KycStatus.ToString(),
                RiskLevel = response.RiskLevel.ToString(),
                RiskCategory = response.RiskCategory,
                RiskSubCategory = response.RiskSubCategory,
                AnnualIncome = response.AnnualIncome,
                SourceOfFunds = response.SourceOfFunds,
                Occupation = response.Occupation,
                EmployerName = response.EmployerName,
                EmploymentStatus = response.EmploymentStatus,
                PurposeOfRelationship = response.PurposeOfRelationship,
                ExpectedMonthlyTransactionVolume = response.ExpectedMonthlyTransactionVolume,
                CustomerSince = response.CustomerSince,
                Addresses = response.Addresses.Select(a => new CustomerDetailResponse.AddressResponse
                {
                    Id = a.Id,
                    Type = a.Type.ToString(),
                    Street = a.Street,
                    City = a.City,
                    State = a.State,
                    ZipCode = a.ZipCode,
                    Country = a.Country,
                    IsPrimary = a.IsPrimary
                }).ToList(),
                Contacts = response.Contacts.Select(c => new CustomerDetailResponse.ContactResponse
                {
                    Id = c.Id,
                    Type = c.Type.ToString(),
                    Email = c.Email,
                    Phone = c.Phone,
                    IsPrimary = c.IsPrimary
                }).ToList(),

                IdentityDocuments = response.IdentityDocuments.Select(i => new CustomerDetailResponse.IdentityDocumentResponse
                {
                    Id = i.Id,
                    Type = i.Type.ToString(),
                    DocumentNumber = i.DocumentNumber,
                    IssuingAuthority = i.IssuingAuthority,
                    IssuingCountry = i.IssuingCountry,
                    IssuedDate = i.IssueDate,
                    ExpiryDate = i.ExpiryDate
                }).ToList()
            };
            return customer;
    }
}
