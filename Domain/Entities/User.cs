using System.Text.RegularExpressions;
using Domain.Common;
using Domain.Enums;

namespace Domain.Entities
{
    public class User : AuditableEntity
    {
        public string FirstName { get; private set; } = null!;
        public string LastName { get; private set; } = null!;
        public string Email { get; private set; } = null!;
        public string PasswordHash { get; private set; } = null!;
        public string Pin { get; private set; } = null!;
        public UserRole Duty { get; private set; }

        public User()
        {
        }

        public static User Create(string firstName, string lastName, string email, string passwordHash, string pin, UserRole duty)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException("First name cannot be empty.", nameof(firstName));

            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException("Last name cannot be empty.", nameof(lastName));

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be empty.", nameof(email));

            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new ArgumentException("Password hash cannot be empty.", nameof(passwordHash));

            if (!Regex.IsMatch(pin, "^[0-9]{4}$"))
            {
                throw new ArgumentException("PIN must consist of exactly 4 digits.", nameof(pin));
            }


            return new User
            {
                FirstName = NormalizeString(firstName),
                LastName = NormalizeString(lastName),
                Email = email.ToLower(),
                PasswordHash = passwordHash,
                Pin = pin,
                Duty = duty
            };
        }


        public void UpdateProfile(string? firstName, string? lastName, UserRole? duty)
        {
            if (!string.IsNullOrWhiteSpace(firstName))
            {
                FirstName = NormalizeString(firstName);
            }

            if (!string.IsNullOrWhiteSpace(lastName))
            {
                LastName = NormalizeString(lastName);
            }

            if (duty.HasValue)
            {
                Duty = duty.Value;
            }
        }

        public void changeEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be empty.", nameof(email));
            Email = email.ToLower();
        }

        public void changePin(string newPin)
        {
            if (!Regex.IsMatch(newPin, "^[0-9]{4}$"))
            {
                throw new ArgumentException("PIN must consist of exactly 4 digits.", nameof(newPin));
            }

            Pin = newPin;
        }

        public void ChangePassword(string newPasswordHash)
        {
            if (string.IsNullOrWhiteSpace(newPasswordHash))
                throw new ArgumentException("Password hash cannot be empty.");

            PasswordHash = newPasswordHash;
        }

        private static string NormalizeString(string value)
        {
            return char.ToUpper(value[0]) + value.Substring(1).ToLower();
        }

    }

}