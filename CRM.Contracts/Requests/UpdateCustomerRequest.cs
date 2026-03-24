using CRM.API.Contracts.Requests.Customers;

namespace Crm.Contracts.Requests
{
    public class UpdateCustomerRequest : CreateCustomerRequest
    {
        public int CifId;
        public bool IsActive;
    }    
}


