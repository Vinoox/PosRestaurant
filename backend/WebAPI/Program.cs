using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data.Seeders;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;


// Dodaj ten using, aby u¿ywaæ klasy OpenApiInfo
using Microsoft.OpenApi.Models;
using WebAPI.Installers;
using WebAPI.Middleware;


var builder = WebApplication.CreateBuilder(args);

// --- SEKcja dodawania serwisów do kontenera ---


builder.Services.AddScoped<ErrorHandlingMiddleware>();
builder.Services.InstallServicesInAssembly(builder.Configuration);

var app = builder.Build();
app.UseMiddleware<ErrorHandlingMiddleware>();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        // Pobieramy potrzebne narzêdzia z kontenera DI.
        var userManager = services.GetRequiredService<UserManager<User>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        RoleSeeder.SeedRolesAsync(roleManager).Wait();

        // --- Definiujemy dane naszego admina bezpoœrednio w kodzie ---
        const string adminEmail = "admin@gmail.com";
        const string adminPassword = "Admin123-";
        const string adminRoleName = "Admin";

        // Krok 1: SprawdŸ, czy rola "Admin" istnieje. Jeœli nie, stwórz j¹.
        if (!await roleManager.RoleExistsAsync(adminRoleName))
        {
            await roleManager.CreateAsync(new IdentityRole(adminRoleName));
        }

        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            adminUser = User.Create("admin", "admin", adminEmail);
            var result = await userManager.CreateAsync(adminUser, adminPassword);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, adminRoleName);
            }
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}


// --- SEKcja konfiguracji potoku HTTP ---

// Warto w³¹czyæ Swaggera tylko w œrodowisku deweloperskim.
if (app.Environment.IsDevelopment())
{
    // === U¯YJ SWAGGERA (Pocz¹tek) ===

    // 3. To w³¹cza middleware, który generuje plik swagger.json z definicj¹ API.
    app.UseSwagger();

    // 4. To w³¹cza middleware, który serwuje interaktywny interfejs u¿ytkownika (UI).
    app.UseSwaggerUI(options =>
    {
        // Ta linia sprawia, ¿e UI Swaggera bêdzie dostêpne pod g³ównym adresem /swagger
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Moje API v1");
    });

    // === U¯YJ SWAGGERA (Koniec) ===
}


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

// Wa¿ne: Jeœli masz kontrolery API (np. klasê dziedzicz¹c¹ po ControllerBase),
// musisz je zmapowaæ, aby Swagger je zobaczy³.
app.MapControllers();

app.Run();