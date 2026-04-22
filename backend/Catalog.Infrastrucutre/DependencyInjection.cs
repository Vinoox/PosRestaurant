using Catalog.Application.Interfaces;
using Catalog.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CatalogDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("CatalogConnection"),
                    b => b.MigrationsAssembly(typeof(CatalogDbContext).Assembly.FullName)));

            services.AddScoped<ICatalogDbContext>(provider => provider.GetRequiredService<CatalogDbContext>());

            return services;
        }
    }
}