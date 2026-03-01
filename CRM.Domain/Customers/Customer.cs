using Crm.Domain.ValueObjects;
using CRM.Domain.Common;
using CRM.Domain.Customers.ValueObjects;
using CRM.Domain.ValueObjects;

namespace CRM.Domain.Customers
{
    public sealed class Customer : AggregateRoot<CustomerId>
    {
        public FullName FullName { get; private set; }

        private readonly List<Address> _addresses = new();
        public IReadOnlyCollection<Address> Addresses => _addresses.AsReadOnly();

        private readonly List<Contact> _contact = new();
        public IReadOnlyCollection<Contact> Contact => _contact.AsReadOnly();

        private readonly List<IdentityDocument> _identityDocuments = new();
        public IReadOnlyCollection<IdentityDocument> IdentityDocuments => _identityDocuments.AsReadOnly();
        public bool IsActive { get; private set; }

        private Customer() { }

        private Customer(CustomerId id, FullName name)
        {
            Id = id;
            Name = name;
        }

        public static Customer CreateNew(FullName name) =>
            new(CustomerId.New(), name);

        // Optional: for reconstitution (used by repository)
        public static Customer Reconstitute(CustomerId id, FullName name) =>
            new(id, name);

        public void UpdateName(FullName newName)
        {
            if (newName == null) throw new DomainException("New name cannot be null.");
            FullName = newName;
        }

        public void AddAddress(Address address)
        {
            if (address == null) 
            throw new DomainException("Address cannot be null.");

            if(address.IsPrimary && _addresses.Any(a => a.IsPrimary))
                throw new DomainException("Only one primary address is allowed.");  

            Address addressToAdd;
            if(!_addresses.Any(a => a.IsPrimary))
                addressToAdd = address.WithIsPrimary(true); // Automatically set first address as primary
            else
                addressToAdd = address;
            
            if(_addresses.Any(a => a.Equals(addressToAdd)))
                throw new DomainException("This address already exists for the customer.");

            _addresses.Add(addressToAdd);   
        }

        public void SetPrimaryAddress(Address address)
        {
            if (address == null) throw new DomainException("Address cannot be null.");
            if (!_addresses.Contains(address)) throw new DomainException("Address does not belong to this customer.");

            var currentPrimary = _addresses.FirstOrDefault(a => a.IsPrimary);

            if (currentPrimary != null && currentPrimary.Equals(address))
                return;

            var updatedAddresses = new List<Address>();
            if (currentPrimary != null)
            {
                var demoted = currentPrimary.WithIsPrimary(false);
                updatedAddresses.Add(demoted);
            }
            var promoted = address.WithIsPrimary(true);
            updatedAddresses.Add(promoted);

            if (currentPrimary != null)
                _addresses.Remove(currentPrimary);
            
            var existingNew = _addresses.FirstOrDefault(a => a.Equals(promoted));
            if (existingNew != null)
                _addresses.Remove(existingNew);

            _addresses.AddRange(updatedAddresses);
        }

        public void UpdateAddress(Address oldAddress, Address newAddress)
        {
            if (oldAddress == null || newAddress == null) throw new DomainException("Addresses cannot be null.");
            if (!_addresses.Contains(oldAddress)) throw new DomainException("Old address does not belong to this customer.");

            var updatedAddress = new Address(
                newAddress.Type,
                oldAddress.ValidFrom,
                oldAddress.ValidTo,
                newAddress.Street,
                newAddress.City,
                newAddress.State,
                newAddress.ZipCode,
                newAddress.Country,
                oldAddress.IsPrimary
            );

            _addresses.Remove(oldAddress);
            _addresses.Add(updatedAddress);
        }
        public void AddContact(Contact contact)
        {
            if (contact == null) throw new DomainException("Contact cannot be null.");
            _contact.Add(contact);

            if(contact.IsPrimary && _contact.Any(c => c.IsPrimary))
                throw new DomainException("Only one primary contact is allowed.");  

            if(!contact.IsPrimary && !_contact.Any(c => c.IsPrimary))
                contact = contact.WithIsPrimary(true); // Automatically set first contact as primary
        }
        public void UpdateContact(Contact oldContact, Contact newContact)
        {
            if (oldContact == null || newContact == null) throw new DomainException("Contacts cannot be null.");
            if (!_contact.Contains(oldContact)) throw new DomainException("Old contact does not belong to this customer.");

            var updatedContact = new Contact(
                newContact.Type,
                newContact.Phone,
                newContact.Email,
                oldContact.ValidFrom,
                oldContact.ValidTo,
                oldContact.IsPrimary
            );

            _contact.Remove(oldContact);
            _contact.Add(updatedContact);
        }
        public void SetPrimaryContact(Contact newPrimaryContact)
        {
            if (newPrimaryContact == null)
                throw new DomainException("Contact cannot be null.");

            if (!_contact.Any(c => c.Equals(newPrimaryContact)))
                throw new DomainException("Contact does not belong to this customer.");

            var currentPrimary = _contact.FirstOrDefault(c => c.IsPrimary);

            if (currentPrimary != null && currentPrimary.Equals(newPrimaryContact))
                return;

            var updatedContacts = new List<Contact>();

            if (currentPrimary != null){
                var demoted = currentPrimary.WithIsPrimary(false);
                updatedContacts.Add(demoted);
            }

            var promoted = newPrimaryContact.WithIsPrimary(true);
            updatedContacts.Add(promoted);

            if (currentPrimary != null)
                _contact.Remove(currentPrimary);
            var existingNew = _contact.FirstOrDefault(c => c.Equals(newPrimaryContact));
            if (existingNew != null)
                _contact.Remove(existingNew);

            _contact.AddRange(updatedContacts);
        }

        public void AddIdentityDocument(IdentityDocument document)
        {
            if (document == null) throw new DomainException("Identity document cannot be null.");
            _identityDocuments.Add(document);
        }
        public void UpdateIdentityDocument(IdentityDocument oldDocument, IdentityDocument newDocument)
        {
            if (oldDocument == null || newDocument == null) throw new DomainException("Identity documents cannot be null.");
            if (!_identityDocuments.Contains(oldDocument)) throw new DomainException("Old document does not belong to this customer.");

            var updatedDocument = new IdentityDocument(
                newDocument.Type,
                newDocument.DocumentNumber,
                newDocument.IssuingCountry,
                newDocument.IssueDate,
                newDocument.ExpiryDate,
                newDocument.FileReference
            );

            _identityDocuments.Remove(oldDocument);
            _identityDocuments.Add(updatedDocument);
        }
        public void RemoveAddress(Address address)
        {
            if (address == null) throw new DomainException("Address cannot be null.");
            _addresses.Remove(address);
        }

        public void RemoveContact(Contact contact)
        {
            if (contact == null) throw new DomainException("Contact cannot be null.");
            _contact.Remove(contact);
        }

        public void RemoveIdentityDocument(IdentityDocument document)
        {
            if (document == null) throw new DomainException("Identity document cannot be null.");
            _identityDocuments.Remove(document);
        }

        public void Deactivate()
        {
            IsActive = false;
        }
        public void Activate()
        {
            IsActive = true;
        }   
    
}