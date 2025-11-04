using Application.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Services
{
    /// <summary>
    /// Implementuje logikę tworzenia i podpisywania tokenów JWT.
    /// </summary>
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly IConfiguration _configuration;

        // Wstrzykujemy IConfiguration, aby mieć dostęp do klucza i innych ustawień z appsettings.json
        public JwtTokenGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(User user, IEnumerable<string> roles)
        {
            // 1. Stwórz listę "oświadczeń" (claims), czyli informacji, które zapiszemy w tokenie.
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id), // Standardowy claim: Subject (Identyfikator użytkownika)
                new Claim(JwtRegisteredClaimNames.Email, user.Email), // Standardowy claim: Email
            };

            // 2. Dodaj role użytkownika do listy oświadczeń.
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // 3. Pobierz super-tajny klucz z appsettings.json i zamień go na bajty.
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            // 4. Stwórz "kredencjały" do podpisania tokenu, używając klucza i algorytmu HMAC-SHA256.
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // 5. Stwórz obiekt tokenu, podając wszystkie potrzebne informacje.
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],       // Kto wydał token (nasza aplikacja)
                audience: _configuration["Jwt:Audience"],   // Dla kogo jest token (nasza aplikacja)
                claims: claims,                             // Lista oświadczeń
                expires: DateTime.UtcNow.AddHours(1),       // Kiedy token wygasa
                signingCredentials: creds                   // Jak go podpisać
            );

            // 6. Zamień obiekt tokenu na finalny, zaszyfrowany string.
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}