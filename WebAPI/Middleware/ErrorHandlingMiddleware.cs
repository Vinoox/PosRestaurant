using Application.Common.Exceptions;
using System.Net;
using System.Text.Json;

namespace WebAPI.Middleware
{
    public class ErrorHandlingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context); // Spróbuj wykonać następny krok w potoku
            }
            catch (Exception ex)
            {
                // Jeśli poleci jakikolwiek błąd, złap go i obsłuż
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var statusCode = HttpStatusCode.InternalServerError; // Domyślnie 500
            object? response = null;

            // Sprawdzamy, jakiego typu jest wyjątek i dopasowujemy odpowiedź
            switch (exception)
            {
                // TO JEST NASZ NOWY PRZYPADEK
                case ValidationException validationException:
                    statusCode = HttpStatusCode.BadRequest; // Kod 400
                    response = new { error = validationException.Message };
                    break;

                case KeyNotFoundException: // Możemy też obsłużyć inne typowe błędy
                    statusCode = HttpStatusCode.NotFound; // Kod 404
                    response = new { error = "Zasób nie został znaleziony." };
                    break;

                // Domyślny przypadek dla wszystkich innych, nieprzewidzianych błędów
                default:
                    response = new { error = "Wystąpił nieoczekiwany błąd serwera." };
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}