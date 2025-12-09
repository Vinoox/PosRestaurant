using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services
{
    public class PinHasher : IPinHasher
    {
        private readonly IPasswordHasher<User> _passwordHasher;

        public PinHasher(IPasswordHasher<User> passwordHasher)
        {
            _passwordHasher = passwordHasher;
        }

        public string Hash(string pin)
        {
            return _passwordHasher.HashPassword(null, pin);
        }

        public bool Verify(string hash, string providedPin)
        {
            var result = _passwordHasher.VerifyHashedPassword(null, hash, providedPin);

            return result == PasswordVerificationResult.Success;
        }
    }
}