using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

namespace WebAPI.Swagger // Lub inna przestrzeń nazw
{
    /// <summary>
    /// Ten filtr jest uruchamiany dla każdego endpointu (operacji) w API.
    /// Jego zadaniem jest sprawdzenie, czy endpoint ma atrybut [Authorize],
    /// i jeśli tak, dodanie do jego dokumentacji ikony kłódki (wymagania bezpieczeństwa).
    /// </summary>
    public class AuthorizeCheckOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // Sprawdzamy, czy metoda w kontrolerze LUB cała klasa kontrolera
            // ma na sobie atrybut [Authorize].
            var hasAuthorizeAttribute = context.MethodInfo.DeclaringType.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any() ||
                                        context.MethodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any();

            // Jeśli endpoint NIE jest chroniony, nic więcej nie robimy.
            if (!hasAuthorizeAttribute)
            {
                return;
            }

            // Jeśli endpoint JEST chroniony, dodajemy do jego dokumentacji
            // informację o możliwych odpowiedziach 401 (Unauthorized) i 403 (Forbidden).
            operation.Responses.TryAdd("401", new OpenApiResponse { Description = "Unauthorized" });
            operation.Responses.TryAdd("403", new OpenApiResponse { Description = "Forbidden" });

            // I co najważniejsze, dodajemy do TEGO KONKRETNEGO endpointu
            // wymaganie bezpieczeństwa, które wyświetli ikonę kłódki.
            operation.Security = new List<OpenApiSecurityRequirement>
            {
                new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer" // Musi pasować do nazwy z AddSecurityDefinition
                            }
                        },
                        new string[] {}
                    }
                }
            };
        }
    }
}