using CRM.Domain.Common;
using ErrorOr;
namespace CRM.Domain.ValueObjects
{
    public sealed class FullName : ValueObject
    {
        public string FirstName { get; }
        public string? MiddleName { get; }
        public string? SecondLastName { get; }
        public string LastName { get; }

        private FullName() { } //EF

        public FullName(string firstName, string lastName, string? middleName, string? secondLastName)
        {
            FirstName = firstName;
            LastName = lastName;
            MiddleName = middleName;
            SecondLastName = secondLastName;
        }

        // ──── Preferred factory method ────
        public static ErrorOr<FullName> Create(string firstName,string lastName,string? middleName= null,string? secondLastName = null)
        {
            var errors = new List<Error>();

            if (string.IsNullOrWhiteSpace(firstName))
            {
                errors.Add(Error.Validation(
                    code:     "FullName.FirstNameRequired",
                    description: "First name is required."));
            }
            else if (firstName.Length is < 2 or > 100)
            {
                errors.Add(Error.Validation(
                    code:     "FullName.FirstNameLength",
                    description: "First name must be between 2 and 100 characters."));
            }

            if (string.IsNullOrWhiteSpace(lastName))
            {
                errors.Add(Error.Validation(
                    code:     "FullName.LastNameRequired",
                    description: "Last name is required."));
            }
            else if (lastName.Length is < 2 or > 100)
            {
                errors.Add(Error.Validation(
                    code:     "FullName.LastNameLength",
                    description: "Last name must be between 2 and 100 characters."));
            }

            if (errors.Count > 0)
            {
                return errors;  // implicit conversion → ErrorOr<FullName>
            }

            return new FullName(
                firstName.Trim(),
                lastName.Trim(),
                middleName?.Trim(),
                secondLastName?.Trim());
        }


        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return FirstName;
            yield return LastName;
            yield return MiddleName ?? string.Empty;
            yield return SecondLastName ?? string.Empty;
        }

    }
}