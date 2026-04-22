using Catalog.Application;
using Catalog.Infrastructure;
using Catalog.Application.Common.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// 1. Rejestracja warstw Czystej Architektury
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// 2. Rejestracja kontrolerów i Swaggera
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 3. Globalna obs³uga b³êdów (Middleware wy³apuj¹cy nasze b³êdy walidacji i domeny)
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
        var exception = exceptionHandlerPathFeature?.Error;

        if (exception is ValidationException validationException)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(new
            {
                Message = "B³¹d walidacji danych",
                Errors = validationException.Errors
            });
        }
        else if (exception is PosRestaurant.Shared.Exceptions.DomainException domainException)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(new { Message = domainException.Message });
        }
        else
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(new { Message = "Wyst¹pi³ nieoczekiwany b³¹d serwera." });
        }
    });
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();