using MediatR;
using Microsoft.AspNetCore.Mvc;
using CRM.API.Contracts.Requests.Customers;
using CRM.Application.Customers.Commands.CreateCustomer;
using CRM.Api.Common;
using CRM.Application.Customers.Queries.GetAllCustomers;
using CRM.Application.Customers.Queries.GetCustomerByCif;
using CRM.Contracts.Requests;
using CRM.Application.Customers.Commands.UpdateCustomerStatus;
using CRM.Application.Customers.Commands.PatchCustomer;
using Crm.Application.Customers.Dtos.Update;
using CRM.Application.Customers.DTOs.Update;
using CRM.Application.Customers.DTOs;
using CRM.Application.Customers.UpdateCustomer;
using CRM.Contracts.Requests.Common;

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
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result= await _mediator.Send(new GetAllCustomersQuery(), cancellationToken);

            return result.Match(
                list => Ok(list),
                errors => Problem(errors)
            );
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(
            CreateCustomerRequest request,
            CancellationToken cancellationToken)
        {
            var command = new CreateCustomerCommand{
                FirstName = request.FirstName,
                MiddleName = request.MiddleName,
                LastName = request.LastName,
                Contacts = request.Contacts.Select(c => new ContactDto(
                                                            Type: c.Type,
                                                            Phone: c.Phone,
                                                            Email: c.Email,
                                                            IsPrimary: c.IsPrimary
                                                        )).ToList(),

                Addresses = request.Addresses.Select(a => new AddressDto(
                                                            Type: a.Type,
                                                            Street: a.AddressLine1,
                                                            City: a.City,
                                                            State: a.State,
                                                            ZipCode: a.PostalCode,
                                                            Country: a.Country,
                                                            IsPrimary: a.IsPrimary
                                                        )).ToList(),

                Identifications = request.IdentityDocuments.Select(d => new IdentificationDto(
                                                                        Type: d.Type,
                                                                        DocumentNumber: d.DocumentNumber,
                                                                        IssuingAuthority: d.IssuingAuthority,
                                                                        IssuingCountry: d.IssuingCountry,
                                                                        IssuedDate: d.IssuedDate,
                                                                        ExpiryDate: d.ExpiryDate
                                                                        )).ToList()
            };

            var result = await _mediator.Send(command, cancellationToken);
            // return Ok(new { id = customerId.Value });
            return result.Match(
                id => CreatedAtAction(nameof(GetById), new { id }, new {id}),
                errors => Problem(errors));
        }

        [HttpGet("GetbyCif/{id}")]
        public async Task<IActionResult> GetById(int id,CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetCustomerByCifQuery(id), cancellationToken);

            return result.Match(
                list => Ok(list),
                errors => Problem(errors)
            );
        }
        
        [HttpPatch("Status/{cifId}")]
        public async Task<IActionResult> UpdateStatus(
            int cifId, 
            UpdateCustomerStatusRequest updateCustomerStatusRequest, 
            CancellationToken cancellationToken)
        {
            var command = new UpdateCustomerStatusCommand(cifId, updateCustomerStatusRequest.IsActive);
            var result = await _mediator.Send(command, cancellationToken);

            return result.Match(_=> NoContent(), errors => Problem(errors));
        }

        [HttpPatch("{id}/FullUpdateCustomer")]
        public async Task<IActionResult> Patch(
            int id, 
            PatchCustomerRequest request,
            CancellationToken cancellationToken)
        {
            var command = new PatchCustomerCommand
            {
                CifId = id,
                Name = request.Name is null ? null :
                    new PatchNameDto(request.Name.FirstName,
                                    request.Name.MiddleName,
                                    request.Name.LastName),
                Contacts = request.Contacts?.Select(c => new UpdateContactDto(c.Id, 
                                                    c.Type,
                                                    c.Phone,
                                                    c.Email,
                                                    c.IsPrimary,
                                                    c.ValidFrom,
                                                    c.ValidTo)).ToList(),

                Addresses = request.Addresses?.Select(a => new UpdateAddressDto(a.Id,
                                                        a.Type,
                                                        a.Street,
                                                        a.City,
                                                        a.State,
                                                        a.ZipCode,
                                                        a.Country,
                                                        a.IsPrimary,
                                                        a.ValidFrom,
                                                        a.ValidTo)).ToList(),
                
                Identifications = request.Identifications?
                    .Select(i => new UpdateIdentificationDto(i.Id,
                                                            i.Type,
                                                            i.DocumentNumber,
                                                            i.IssuingAuthority,
                                                            i.IssuingCountry,
                                                            i.IssuedDate,
                                                            i.ExpiryDate)).ToList()
            };
            var result = await _mediator.Send(command);
            if(result.IsError)
                return Problem(result.Errors);
            
            var updated = await _mediator.Send(new GetCustomerByCifQuery(id), cancellationToken);
            return updated.Match(
                customer => Ok(customer),
                errors => Problem(errors)
            );
        }

        [HttpPut("{id}/Name")]
        public async Task<IActionResult> UpdateName(
            int id, 
            NameRequestModel request, 
            CancellationToken cancellationToken
        )
        {
            var command = new UpdateCustomerNameCommand
            {
                CifId = id,
                FirstName = request.FirstName,
                MiddleName = request.MiddleName,
                LastName = request.LastName
            };

            var result = await _mediator.Send(command,cancellationToken);
            if(result.IsError)
                return Problem(result.Errors);
            
            var updated = await _mediator.Send(new GetCustomerByCifQuery(id),cancellationToken);
            return updated.Match(
                customer => Ok(customer),
                errors => Problem(errors));
        }

        [HttpPut("{id}/Contact")]
        public async Task<IActionResult> UpdateContact(
            int id, 
            UpdateCustomerContactsRequest request,
            CancellationToken cancellationToken)
        {
            var command = new UpdateCustomerContactsCommand
            {
                CifId = id,
                Contacts = request.Contacts.Select(c => new UpdateContactDto(
                    c.Id,
                    c.Type,
                    c.Phone,
                    c.Email,
                    c.IsPrimary,
                    c.ValidFrom,
                    c.ValidTo

                )).ToList()
            };
            var result = await _mediator.Send(command, cancellationToken);

            if(result.IsError)
                return Problem(result.Errors);
            
            var updated = await _mediator.Send(new GetCustomerByCifQuery(id), cancellationToken);
            return updated.Match(
                customer => Ok(customer.Contacts),
                errors => Problem(errors)
            );
        }

        [HttpPut("{id}/Address")]
        public async Task<IActionResult> UpdateAddress(
            int id,
            UpdateCustomerAddressesRequest request,
            CancellationToken cancellationToken
        )
        {
            var command = new UpdateCustomerAddressesCommand
            {
                CifId = id,
                Addresses = request.Addresses.Select(a => new UpdateAddressDto(
                    a.Id,
                    a.Type,
                    a.Street,
                    a.City,
                    a.State,
                    a.ZipCode,
                    a.Country,
                    a.IsPrimary,
                    a.ValidFrom,
                    a.ValidTo
                )).ToList()
            };
            var result = await _mediator.Send(command ,cancellationToken);
            if(result.IsError)
                return Problem(result.Errors);

            var update = await _mediator.Send(new GetCustomerByCifQuery(id), cancellationToken );
            return update.Match(
                customer => Ok(customer.Addresses),
                errors => Problem(errors)
            );

        }

        [HttpPut("{id}/Identification")]
        public async Task<IActionResult> UpdateIdentification(
            int id,
            UpdateCustomerIdentificationsRequest request,
            CancellationToken cancellationToken
        )
        {
            var command = new UpdateCustomerIdentificationsCommand
            {
                CifId = id,
                Identifications = request.IdentityDocuments.Select(i => new UpdateIdentificationDto(
                    i.Id,
                    i.Type,
                    i.DocumentNumber,
                    i.IssuingAuthority,
                    i.IssuingCountry,
                    i.IssuedDate,
                    i.ExpiryDate
                )).ToList()
            };
            var result = await _mediator.Send(command,cancellationToken);
            if(result.IsError)
                return Problem(result.Errors);

            var updated = await _mediator.Send(new GetCustomerByCifQuery(id), cancellationToken);
            return updated.Match(
                customer => Ok(customer.IdentityDocuments),
                errors => Problem(errors)
            );
        }
        
    }
}   