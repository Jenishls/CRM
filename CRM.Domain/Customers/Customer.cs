using CRM.Domain.Customers.ValueObjects;
using CRM.Domain.Common;
using CRM.Domain.Customers.Entites;
using CRM.Domain.Entities;
using CRM.Domain.Enums;

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
        
        public CustomerType CustomerType { get; private set; }
        public CustomerStatus CustomerStatus { get; private set; }
        public KycStatus KycStatus { get; private set; }
        public RiskLevel RiskLevel { get; private set; }
        public string? RiskCategory { get; private set; }
        public string? RiskSubCategory { get; private set; }
        public decimal? AnnualIncome { get; private set; }
        public string? SourceOfFunds { get; private set; }
        public string? Occupation { get; private set; }
        public string? EmployerName { get; private set; }
        public string? EmploymentStatus { get; private set; }
        public string? PurposeOfRelationship { get; private set; }
        public decimal? ExpectedMonthlyTransactionVolume { get; private set; }
        public DateTime CustomerSince { get; private set; }
        public bool IsActive { get; private set; }
        public byte[] RowVersion { get; private set; } = default!; 

        private Customer() { }

        private Customer(
            CustomerId id,
            int cifId,
            FullName name)
            : base(id)
        {
            CifId = cifId;
            FullName = name;
            CustomerType = CustomerType.Individual;
            CustomerStatus = CustomerStatus.Active;
            KycStatus = KycStatus.NotStarted;
            RiskLevel = RiskLevel.Unknown;
            CustomerSince = DateTime.UtcNow;
            IsActive = true;
        }

        

        public static Customer CreateNew
                (  FullName name, 
                    IEnumerable<Contact> primaryContacts,
                    IEnumerable<Address> primaryAddresses,
                    IEnumerable<IdentityDocument> primaryIdentifications,
                    CustomerStatus customerStatus = CustomerStatus.Active,
                    KycStatus kycStatus = KycStatus.NotStarted,
                    RiskLevel riskLevel = RiskLevel.Unknown,
                    string? riskCategory = null,
                    string? riskSubCategory = null,
                    decimal? annualIncome = null,
                    string? sourceOfFunds = null,
                    string? occupation = null,
                    string? employerName = null,
                    string? employmentStatus = null,
                    string? purposeOfRelationship = null,
                    decimal? expectedMonthlyTransactionVolume = null,
                    DateTime? customerSince = null)
        {
            if (name == null) throw new DomainException("Customer name cannot be null.");

            var customer = new Customer(
                CustomerId.New(),
                0, // CifId will be set by the persistence layer
                name);

            foreach (var contact in (primaryContacts ?? throw new DomainException("At least one contact is required.")).OrderByDescending(c => c.IsPrimary))
                customer.AddContact(contact);

            foreach (var address in (primaryAddresses ?? throw new DomainException("At least one address is required.")).OrderByDescending(a => a.IsPrimary))
                customer.AddAddress(address);

            foreach (var identification in primaryIdentifications ?? throw new DomainException("At least one identity document is required."))
                customer.AddIdentityDocument(identification);

            if (!customer.Contacts.Any())
                throw new DomainException("At least one contact is required.");

            if (!customer.Addresses.Any())
                throw new DomainException("At least one address is required.");

            if (!customer.IdentityDocuments.Any())
                throw new DomainException("At least one identity document is required.");

            customer.UpdatePersonalProfile(
                customerStatus,
                kycStatus,
                riskLevel,
                riskCategory,
                riskSubCategory,
                annualIncome,
                sourceOfFunds,
                occupation,
                employerName,
                employmentStatus,
                purposeOfRelationship,
                expectedMonthlyTransactionVolume,
                customerSince ?? DateTime.UtcNow);

            return customer;
        }

        public void UpdatePersonalProfile(
            CustomerStatus customerStatus,
            KycStatus kycStatus,
            RiskLevel riskLevel,
            string? riskCategory,
            string? riskSubCategory,
            decimal? annualIncome,
            string? sourceOfFunds,
            string? occupation,
            string? employerName,
            string? employmentStatus,
            string? purposeOfRelationship,
            decimal? expectedMonthlyTransactionVolume,
            DateTime customerSince)
        {
            if (annualIncome < 0)
                throw new DomainException("Annual income cannot be negative.");

            if (expectedMonthlyTransactionVolume < 0)
                throw new DomainException("Expected monthly transaction volume cannot be negative.");

            CustomerType = CustomerType.Individual;
            CustomerStatus = customerStatus;
            KycStatus = kycStatus;
            RiskLevel = riskLevel;
            RiskCategory = NormalizeOptional(riskCategory);
            RiskSubCategory = NormalizeOptional(riskSubCategory);
            AnnualIncome = annualIncome;
            SourceOfFunds = NormalizeOptional(sourceOfFunds);
            Occupation = NormalizeOptional(occupation);
            EmployerName = NormalizeOptional(employerName);
            EmploymentStatus = NormalizeOptional(employmentStatus);
            PurposeOfRelationship = NormalizeOptional(purposeOfRelationship);
            ExpectedMonthlyTransactionVolume = expectedMonthlyTransactionVolume;
            CustomerSince = customerSince;
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

        public void UpdateAddress(Guid addressId, Address newAddress)
        {
            if (newAddress == null) throw new DomainException("Address cannot be null.");

            var existing = _addresses.FirstOrDefault(c => c.Id == addressId);
            if (existing is null)
                throw new DomainException("Address does not belong to this customer.");

            _addresses.Remove(existing);
            _addresses.Add(newAddress.WithId(existing.Id));
        }
        public void AddContact(Contact contact)
        {
            if (contact == null) throw new DomainException("Contact cannot be null.");

            if(contact.IsPrimary && _contacts.Any(c => c.IsPrimary))
                throw new DomainException("Only one primary contact is allowed.");  

            Contact contactToAdd;
            if(!contact.IsPrimary && !_contacts.Any(c => c.IsPrimary))
                contactToAdd = contact.WithIsPrimary(true); // Automatically set first contact as primary
            else
                contactToAdd = contact;

            _contacts.Add(contactToAdd);
        }
        public void UpdateContact(Guid contactId, Contact newContact)
        {
            if (newContact == null) throw new DomainException("Contact cannot be null.");

            var existing = _contacts.FirstOrDefault(c => c.Id == contactId);
            if (existing is null)
                throw new DomainException("Contact does not belong to this customer.");
            
            _contacts.Remove(existing);
            _contacts.Add(newContact.WithId(existing.Id));
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
        public void UpdateIdentityDocument(Guid documentId, IdentityDocument newDocument)
        {
            if (newDocument == null) throw new DomainException("Identity document cannot be null.");

            var existing = _identityDocuments.FirstOrDefault(c => c.Id == documentId);

            if (existing is null)
                throw new DomainException("Document does not belong to this customer.");

            _identityDocuments.Remove(existing);
            _identityDocuments.Add(newDocument.WithId(existing.Id));
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

        private static string? NormalizeOptional(string? value)
            => string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    
    }
}
