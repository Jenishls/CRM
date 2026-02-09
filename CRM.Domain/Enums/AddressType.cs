namespace CRM.Domain.Enums
{
    public enum AddressType
    {
    Residential     = 1,    // home / current living address (usually primary for individuals)
    Mailing         = 2,    // correspondence / where to send statements
    Registered      = 3,    // official registered office (most important for organizations)
    Work            = 4,
    Temporary       = 5,
    Previous        = 6,    // historical – often kept for audit / proof of address chain
    Other           = 99
    }
}