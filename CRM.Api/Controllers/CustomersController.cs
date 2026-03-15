using MediatR;
using Microsoft.AspNetCore.Mvc;
using CRM.API.Contracts.Requests.Customers;
using CRM.Application.Customers.Commands.CreateCustomer;
using CRM.Application.Common.Utilities;
using CRM.Domain.Enums;
using CRM.Api.Common;

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

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            // Placeholder for getting all customers
            return Ok(new { Message = "Get all customers - Not implemented yet." });
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
            // return CreatedAtAction(nameof(GetById), new { id = customerId }, new { Id = customerId });
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id,CancellationToken ct)
        {
            // Placeholder for getting a customer by ID
            return Ok();
        }
    }
}   