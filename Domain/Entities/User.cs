using System.Text.RegularExpressions;
using Domain.Common;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; private set; } = null!;
        public string LastName { get; private set; } = null!;
        public string PinHash { get; private set; } = null!;

        private readonly List<StaffAssignment> _staffAssignments = new();
        public IReadOnlyCollection<StaffAssignment> StaffAssignments => _staffAssignments.AsReadOnly();
        private User(){}

        public static User Create(string firstName, string lastName, string email)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException("First name cannot be empty.", nameof(firstName));

            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException("Last name cannot be empty.", nameof(lastName));

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be empty.", nameof(email));

            return new User
            {
                FirstName = NormalizeString(firstName),
                LastName = NormalizeString(lastName),
                Email = email.ToLower(),
                UserName = email.ToLower()
            };
        }

        public void UpdateFirstName(string newFirstName)
        {
            if (string.IsNullOrWhiteSpace(newFirstName))
                throw new DomainException("Imię nie może być puste");

            FirstName = NormalizeString(newFirstName);
        }

        public void UpdateLastName(string newLastName)
        {
            if (string.IsNullOrWhiteSpace(newLastName))
                throw new DomainException("Nazwisko nie może być puste");

            LastName = NormalizeString(newLastName);
        }

        public void SetPinHash(string newPinHash)
        {
            if (string.IsNullOrWhiteSpace(newPinHash))
                throw new ArgumentException("PIN hash cannot be empty.", nameof(newPinHash));

            PinHash = newPinHash;
        }

        private static string NormalizeString(string value)
        {
            return char.ToUpper(value[0]) + value.Substring(1).ToLower();
        }
    }
}