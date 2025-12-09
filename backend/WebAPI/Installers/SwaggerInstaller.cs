using Microsoft.OpenApi.Models;
using WebAPI.Swagger;

namespace WebAPI.Installers
{
    public class SwaggerInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Moje API",
                    Description = "Dokumentacja dla mojego pierwszego Web API w ASP.NET Core"
                });

                options.EnableAnnotations();

                // --- POCZĄTEK NOWEGO FRAGMENTU ---
                // Ten kod "uczy" Swaggera, jak obsługiwać uwierzytelnianie za pomocą tokenów JWT.

                // Krok 1: Definiujemy, jak wygląda nasz schemat bezpieczeństwa.
                // Mówimy Swaggerowi, że używamy uwierzytelniania typu "Bearer Token",
                // które jest przekazywane w nagłówku HTTP o nazwie "Authorization".
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Wprowadź token JWT w formacie: Bearer {token}",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                options.OperationFilter<AuthorizeCheckOperationFilter>();
            });
        }
    }
}