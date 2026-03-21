using CRM.Domain.Customers.ValueObjects;
using CRM.Domain.Common;

namespace CRM.Domain.Customers
{
    public sealed class Customer : AggregateRoot<CustomerId>
    {
        public int CifId { get; private set; }
        public FullName FullName { get; private set; }
        private readonly List<Address> _addresses = new();
        public IReadOnlyCollection<Address> Addresses => _addresses.AsReadOnly();
        private readonly List<Contact> _contacts = new();
        public IReadOnlyCollection<Contact> Contacts => _contacts.AsReadOnly();

        private readonly List<IdentityDocument> _identityDocuments = new();
        public IReadOnlyCollection<IdentityDocument> IdentityDocuments => _identityDocuments.AsReadOnly();
        public bool IsActive { get; private set; }

        private Customer() { }

        private Customer(
            CustomerId id,
            int cifId,
            FullName name,
            IEnumerable<Contact> primaryContacts,
            IEnumerable<Address> primaryAddresses,
            IEnumerable<IdentityDocument> primaryIdentifications)
            : base(id)
        {
            CifId = cifId;
            FullName = name;
            _contacts = primaryContacts?.ToList() ?? new List<Contact>();
            _addresses = primaryAddresses?.ToList() ?? new List<Address>();
            _identityDocuments = primaryIdentifications?.ToList() ?? new List<IdentityDocument>();
        }

        

        public static Customer CreateNew
                (  FullName name, 
                    IEnumerable<Contact> primaryContacts,
                    IEnumerable<Address> primaryAddresses,
                    IEnumerable<IdentityDocument> primaryIdentifications)
        {
            return new Customer(

                CustomerId.New(),
                0, // CifId will be set by the persistence layer
                name,
                primaryContacts,
                primaryAddresses,
                primaryIdentifications
            );
            
        }

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

            var updatedAddress = Address.Create(
                newAddress.Type,
                newAddress.Street,
                newAddress.City,
                newAddress.State,
                newAddress.ZipCode,
                newAddress.Country,
                newAddress.IsPrimary,
                newAddress.ValidFrom,
                newAddress.ValidTo
            );

            _addresses.Remove(oldAddress);
            _addresses.Add(updatedAddress);
        }
        public void AddContact(Contact contact)
        {
            if (contact == null) throw new DomainException("Contact cannot be null.");
            _contacts.Add(contact);

            if(contact.IsPrimary && _contacts.Any(c => c.IsPrimary))
                throw new DomainException("Only one primary contact is allowed.");  

            if(!contact.IsPrimary && !_contacts.Any(c => c.IsPrimary))
                contact = contact.WithIsPrimary(true); // Automatically set first contact as primary
        }
        public void UpdateContact(Contact oldContact, Contact newContact)
        {
            if (oldContact == null || newContact == null) throw new DomainException("Contacts cannot be null.");
            if (!_contacts.Contains(oldContact)) throw new DomainException("Old contact does not belong to this customer.");

            var updatedContact = Contact.Create(
                newContact.Type,
                newContact.Phone,
                newContact.Email,
                oldContact.ValidFrom,
                oldContact.ValidTo,
                oldContact.IsPrimary
            );

            _contacts.Remove(oldContact);
            _contacts.Add(updatedContact);
        }
        public void SetPrimaryContact(Contact newPrimaryContact)
        {
            if (newPrimaryContact == null)
                throw new DomainException("Contact cannot be null.");

            if (!_contacts.Any(c => c.Equals(newPrimaryContact)))
                throw new DomainException("Contact does not belong to this customer.");

            var currentPrimary = _contacts.FirstOrDefault(c => c.IsPrimary);

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
                _contacts.Remove(currentPrimary);
            var existingNew = _contacts.FirstOrDefault(c => c.Equals(newPrimaryContact));
            if (existingNew != null)
                _contacts.Remove(existingNew);

            _contacts.AddRange(updatedContacts);
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

            var updatedDocument = IdentityDocument.Create(
                newDocument.Type,
                newDocument.DocumentNumber,
                newDocument.IssuingAuthority,
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
            _contacts.Remove(contact);
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
}