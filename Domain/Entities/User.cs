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
        public UserRole Duty { get; private set; }

        public int? RestaurantId { get; set; }
        public Restaurant? Restaurant { get; set; }

        private User()
        {
        }

        public static User Create(string firstName, string lastName, string email, UserRole duty)
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
                Duty = duty,
                Email = email.ToLower(),
                UserName = email.ToLower()
            };
        }

        public void UpdateProfile(string? firstName, string? lastName, UserRole? duty)
        {
            if (!string.IsNullOrWhiteSpace(firstName))
                FirstName = NormalizeString(firstName);

            if (!string.IsNullOrWhiteSpace(lastName))
                LastName = NormalizeString(lastName);

            if (duty.HasValue)
                Duty = duty.Value;
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