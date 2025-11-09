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
        public string? PinHash { get; private set; } = null!;
        public ICollection<StaffAssignment> StaffAssignments { get; set; } = new List<StaffAssignment>();
        private User()
        {
        }

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

        public void UpdateProfile(string? firstName, string? lastName)
        {
            if (!string.IsNullOrWhiteSpace(firstName))
                FirstName = NormalizeString(firstName);

            if (!string.IsNullOrWhiteSpace(lastName))
                LastName = NormalizeString(lastName);
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