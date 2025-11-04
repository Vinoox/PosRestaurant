using Microsoft.OpenApi.Models;

namespace WebAPI.Installers
{
    public class SwaggerInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            // === DODAJ SWAGGERA (Początek) ===

            // 1. Ten serwis jest potrzebny, aby Swagger mógł odkrywać i opisywać endpointy API.
            services.AddEndpointsApiExplorer();

            // 2. To jest główny serwis, który generuje dokumentację Swaggera.
            services.AddSwaggerGen(options =>
            {
                // Ta część jest opcjonalna, ale pozwala zdefiniować tytuł, wersję itp.
                // co będzie widoczne na stronie UI Swaggera.
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Moje API",
                    Description = "Dokumentacja dla mojego pierwszego Web API w ASP.NET Core"
                });

                options.EnableAnnotations();
            });

            // === DODAJ SWAGGERA (Koniec) ===
        }
    }
}
