using Application.Common.Exceptions;
using System.Net;
using System.Security;
using System.Text.Json;

namespace WebAPI.Middleware
{
    public class ErrorHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Wystąpił błąd podczas przetwarzania żądania.");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            HttpStatusCode statusCode;
            string message;

            switch (exception)
            {
                case NotFoundException:
                    statusCode = HttpStatusCode.NotFound; // 404
                    message = exception.Message;
                    break;

                case InvalidOperationException:
                    statusCode = HttpStatusCode.BadRequest; // 400
                    message = exception.Message;
                    break;

                case BadRequestException:
                    statusCode = HttpStatusCode.BadRequest; // 400
                    message = exception.Message;
                    break;

                case UnauthorizedAccessException:
                    statusCode = HttpStatusCode.Unauthorized; // 401
                    message = exception.Message;
                    break;

                case SecurityException:
                    statusCode = HttpStatusCode.Forbidden; //403
                    message = exception.Message;
                    break;

                default:
                    statusCode = HttpStatusCode.InternalServerError; // 500
                    message = "Wystąpił wewnętrzny błąd serwera. Skontaktuj się z administratorem.";
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var result = JsonSerializer.Serialize(new { error = message });
            await context.Response.WriteAsync(result);
        }
    }
}