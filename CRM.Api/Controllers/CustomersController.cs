using MediatR;
using Microsoft.AspNetCore.Mvc;
using CRM.API.Contracts.Requests.Customers;
using CRM.Application.Customers.Commands.CreateCustomer;
using CRM.Api.Common;
using CRM.Application.Customers.Queries.GetAllCustomers;
using CRM.Application.Customers.Queries.GetCustomerByCif;
using CRM.Contracts.Requests;
using CRM.Application.Customers.Commands.UpdateCustomerStatus;

namespace CRM.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ApiController
    {
        private readonly IMediator _mediator;
        public CustomersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var result= await _mediator.Send(new GetAllCustomersQuery());

            return result.Match(
                list => Ok(list),
                errors => Problem(errors)
            );
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(CreateCustomerRequest request, CancellationToken ct)
        {
            var command = new CreateCustomerCommand{
                FirstName = request.FirstName,
                MiddleName = request.MiddleName,
                LastName = request.LastName,
                Contacts = request.Contacts.Select(c =>
                {
                    return new CreateCustomerCommand.ContactModel(
                                        Type: c.Type,
                                        Phone: c.Phone,
                                        Email: c.Email,
                                        IsPrimary: c.IsPrimary
                                    );
                }).ToList(),
                Addresses = request.Addresses.Select(a =>
                {
                    return new CreateCustomerCommand.AddressModel(
                                        Type: a.Type,
                                        Street: a.AddressLine1,
                                        City: a.City,
                                        State: a.State,
                                        ZipCode: a.PostalCode,
                                        Country: a.Country,
                                        IsPrimary: a.IsPrimary
                                    );
                }).ToList(),
                Identifications = request.IdentityDocuments.Select(d =>
                {
                    return new CreateCustomerCommand.IdentificationModel(Type: d.Type,
                                                                         DocumentNumber: d.DocumentNumber,
                                                                         IssuingAuthority: d.IssuingAuthority,
                                                                         IssuingCountry: d.IssuingCountry,
                                                                         IssuedDate: d.IssuedDate,
                                                                         ExpiryDate: d.ExpiryDate);
                }).ToList()
            };

            var result = await _mediator.Send(command, ct);
            // return Ok(new { id = customerId.Value });
            return result.Match(
                id => CreatedAtAction(nameof(GetById), new { id }, id),
                errors => Problem(errors)
            );
        }

        [HttpGet("GetbyCif/{id}")]
        public async Task<IActionResult> GetById(int id,CancellationToken ct)
        {
            var result = await _mediator.Send(new GetCustomerByCifQuery(id));

            return result.Match(
                list => Ok(list),
                errors => Problem(errors)
            );
        }
        [HttpPatch("Status/{cifId}")]
        public async Task<IActionResult> UpdateStatus(
            int cifId, 
            UpdateCustomerStatusRequest updateCustomerStatusRequest, 
            CancellationToken ct)
        {
            var command = new UpdateCustomerStatusCommand(cifId, updateCustomerStatusRequest.IsActive);
            var result = await _mediator.Send(command, ct);

            return result.Match(_=> NoContent(), errors => Problem(errors));
        }
    }
}   