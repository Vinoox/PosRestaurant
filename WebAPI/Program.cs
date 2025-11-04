using Domain.Interfaces;
using Infrastructure.Repositories;
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

// Ten blok jest na swoim miejscu - obs³uguje b³êdy w œrodowisku produkcyjnym.
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