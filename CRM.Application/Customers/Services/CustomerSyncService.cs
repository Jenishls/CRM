using Crm.Application.Customers.Dtos.Update;
using CRM.Application.Common.Errors;
using CRM.Application.Common.Utilities;
using CRM.Application.Customers.DTOs.Update;
using CRM.Domain.Customers;
using CRM.Domain.Customers.Entites;
using CRM.Domain.Entities;
using CRM.Domain.Enums;
using ErrorOr;
using MediatR;

namespace CRM.Application.Customers.Services
{

public class CustomerSyncService
{
    public ErrorOr<Unit> SyncContacts(
        Customer customer,
        List<UpdateContactDto> incomingContacts)
    {
        // Pre-validate
        var remaining = CountRemaining(
            customer.Contacts.Select(c => c.Id),
            incomingContacts.Where(c => c.Id.HasValue).Select(c => c.Id!.Value),
            incomingContacts.Count(c => c.Id is null));

        if (remaining == 0)
            return CustomerErrors.MinimumContactRequired();

        var removedContacts = customer.Contacts
            .Where(c => !incomingContacts.Any(ic => ic.Id == c.Id))
            .ToList();

        var removingPrimary = removedContacts.Any(c => c.IsPrimary);
        var incomingHasPrimary = incomingContacts.Any(c => c.IsPrimary);

        if (removingPrimary && !incomingHasPrimary)
            return CustomerErrors.PrimaryContactRequired();

        var expiredContact = incomingContacts
            .FirstOrDefault(c => c.ValidTo.HasValue && c.ValidTo < DateTime.UtcNow);

        if (expiredContact?.Id is not null)
            return CustomerErrors.ContactExpired(expiredContact.Id.Value);

        // Remove
        foreach (var contact in removedContacts)
            customer.RemoveContact(contact);

        // Add or Update
        foreach (var contactDto in incomingContacts.OrderBy(c => c.IsPrimary))
        {
            var existing = customer.Contacts
                .FirstOrDefault(c => c.Id == contactDto.Id);

            if (existing is null)
            {
                var newContact = Contact.Create(
                    EnumParser.Parse<ContactType>(contactDto.Type),
                    contactDto.Phone,
                    contactDto.Email,
                    contactDto.ValidFrom,
                    contactDto.ValidTo,
                    contactDto.IsPrimary);

                customer.AddContact(newContact);
            }
            else
            {
                var updated = Contact.Create(
                    EnumParser.Parse<ContactType>(contactDto.Type),
                    contactDto.Phone,
                    contactDto.Email,
                    contactDto.ValidFrom,
                    contactDto.ValidTo,
                    contactDto.IsPrimary);

                customer.UpdateContact(existing.Id, updated);
            }

            if (contactDto.IsPrimary)
            {
                var target = customer.Contacts
                    .FirstOrDefault(c => c.Phone == contactDto.Phone);

                if (target is not null)
                    customer.SetPrimaryContact(target);
            }
        }

        return Unit.Value;
    }

    public ErrorOr<Unit> SyncAddresses(
        Customer customer,
        List<UpdateAddressDto> incomingAddresses)
    {
        var remaining = CountRemaining(
            customer.Addresses.Select(a => a.Id),
            incomingAddresses.Where(a => a.Id.HasValue).Select(a => a.Id!.Value),
            incomingAddresses.Count(a => a.Id is null));

        if (remaining == 0)
            return CustomerErrors.MinimumAddressRequired();

        var removedAddresses = customer.Addresses
            .Where(a => !incomingAddresses.Any(ia => ia.Id == a.Id))
            .ToList();

        var removingPrimary = removedAddresses.Any(a => a.IsPrimary);
        var incomingHasPrimary = incomingAddresses.Any(a => a.IsPrimary);

        if (removingPrimary && !incomingHasPrimary)
            return CustomerErrors.PrimaryAddressRequired();

        var expiredAddress = incomingAddresses
            .FirstOrDefault(a => a.ValidTo.HasValue && a.ValidTo < DateTime.UtcNow);

        if (expiredAddress?.Id is not null)
            return CustomerErrors.AddressExpired(expiredAddress.Id.Value);

        foreach (var address in removedAddresses)
            customer.RemoveAddress(address);

        foreach (var addressDto in incomingAddresses.OrderBy(a => a.IsPrimary))
        {
            var existing = customer.Addresses
                .FirstOrDefault(a => a.Id == addressDto.Id);

            if (existing is null)
            {
                var newAddress = Address.Create(
                    EnumParser.Parse<AddressType>(addressDto.Type),
                    addressDto.Street,
                    addressDto.City,
                    addressDto.State,
                    addressDto.ZipCode,
                    addressDto.Country,
                    addressDto.IsPrimary,
                    addressDto.ValidFrom,
                    addressDto.ValidTo);

                customer.AddAddress(newAddress);
            }
            else
            {
                var updated = Address.Create(
                    EnumParser.Parse<AddressType>(addressDto.Type),
                    addressDto.Street,
                    addressDto.City,
                    addressDto.State,
                    addressDto.ZipCode,
                    addressDto.Country,
                    addressDto.IsPrimary,
                    addressDto.ValidFrom,
                    addressDto.ValidTo);

                customer.UpdateAddress(existing.Id, updated);
            }

            if (addressDto.IsPrimary)
            {
                var target = customer.Addresses
                    .FirstOrDefault(a => a.Street == addressDto.Street
                        && a.City == addressDto.City);

                if (target is not null)
                    customer.SetPrimaryAddress(target);
            }
        }

        return Unit.Value;
    }

        public ErrorOr<Unit> SyncIdentifications(
        Customer customer,
        List<UpdateIdentificationDto> incomingDocuments)
        {
            var remaining = CountRemaining(
                customer.IdentityDocuments.Select(i => i.Id),
                incomingDocuments.Where(i => i.Id.HasValue).Select(i => i.Id!.Value),
                incomingDocuments.Count(i => i.Id is null));

            if (remaining == 0)
                return CustomerErrors.MinimumIdentificationRequired();

            var expiredDoc = incomingDocuments
                .FirstOrDefault(i => i.ExpiryDate.HasValue && i.ExpiryDate < DateTime.UtcNow);

            if (expiredDoc?.Id is not null)
                return CustomerErrors.IdentificationExpired(expiredDoc.Id.Value);

            var documentsToRemove = customer.IdentityDocuments
                .Where(i => !incomingDocuments.Any(id => id.Id == i.Id))
                .ToList();

            foreach (var document in documentsToRemove)
                customer.RemoveIdentityDocument(document);

            foreach (var identificationDto in incomingDocuments)
            {
                var existing = customer.IdentityDocuments
                    .FirstOrDefault(i => i.Id == identificationDto.Id);

                if (existing is null)
                {
                    var newDocument = IdentityDocument.Create(
                        EnumParser.Parse<DocumentType>(identificationDto.Type),
                        identificationDto.DocumentNumber,
                        identificationDto.IssuingAuthority,
                        identificationDto.IssuingCountry,
                        identificationDto.IssuedDate,
                        identificationDto.ExpiryDate,
                        fileReference: null);

                    customer.AddIdentityDocument(newDocument);
                }
                else
                {
                    var updated = IdentityDocument.Create(
                        EnumParser.Parse<DocumentType>(identificationDto.Type),
                        identificationDto.DocumentNumber,
                        identificationDto.IssuingAuthority,
                        identificationDto.IssuingCountry,
                        identificationDto.IssuedDate,
                        identificationDto.ExpiryDate,
                        existing.FileReference);        // ← preserve file reference

                    customer.UpdateIdentityDocument(existing.Id, updated);
                }
            }

            return Unit.Value;
        }

        private static int CountRemaining(
            IEnumerable<Guid> existingIds,
            IEnumerable<Guid> incomingExistingIds,
            int newRecordsCount)
        {
            var keptCount = existingIds.Count(id => incomingExistingIds.Contains(id));
            return keptCount + newRecordsCount;
        }
    }
}
