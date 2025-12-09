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
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly IConfiguration _configuration;

        // Wstrzykujemy IConfiguration, aby mieć dostęp do klucza i innych ustawień z appsettings.json
        public JwtTokenGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateAuthenticationToken(User user, IEnumerable<string> globalRoles)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            };

            foreach (var role in globalRoles)
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
                expires: DateTime.UtcNow.AddMinutes(5),       // Kiedy token wygasa
                signingCredentials: creds                   // Jak go podpisać
            );
            // 6. Zamień obiekt tokenu na finalny, zaszyfrowany string.
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateContextualToken(User user, int restaurantId, IEnumerable<string> restaurantRoles)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("restaurantId", restaurantId.ToString())
            };

            foreach (var role in restaurantRoles)
            {
                claims.Add(new Claim("restaurant_role", role));
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
                expires: DateTime.UtcNow.AddHours(12),       // Kiedy token wygasa
                signingCredentials: creds                   // Jak go podpisać
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}