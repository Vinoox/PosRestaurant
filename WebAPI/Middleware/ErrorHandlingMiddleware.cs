using System.Net;
using System.Runtime.Intrinsics.Arm;
using System.Security;
using System.Text.Json;
using Application.Common.Exceptions;
using Domain.Common;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
                _logger.LogError(ex, "Nieobsłużony wyjątek: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var (statusCode, title, detail) = MapExceptionToResponse(exception);

            var problemDetails = new ProblemDetails
            {
                Status = (int)statusCode,
                Title = title,
                Detail = detail,
                Type = $"https://httpstatuses.com/{(int)statusCode}",
                Instance = context.Request.Path
            };

            if (exception is FluentValidation.ValidationException validationEx)
            {
                var errors = validationEx.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.ErrorMessage).ToArray());

                problemDetails.Extensions["errors"] = errors;
            }

            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = (int)statusCode;

            var result = JsonSerializer.Serialize(problemDetails);
            await context.Response.WriteAsync(result);
        }

        private static (HttpStatusCode statusCode, string title, string detail) MapExceptionToResponse(Exception exception)
        {
            return exception switch
            {
                DomainException ex =>
                    (HttpStatusCode.BadRequest, "Naruszenie reguł domeny", ex.Message),

                BadRequestException ex =>
                    (HttpStatusCode.BadRequest, "Nieprawidłowe żądanie", ex.Message),

                NotFoundException ex =>
                    (HttpStatusCode.NotFound, "Zasób nie znaleziony", ex.Message),

                ValidationException ex =>
                    (HttpStatusCode.BadRequest, "Błąd walidacji", "Wystąpiły błędy walidacji danych."),

                UnauthorizedAccessException ex =>
                    (HttpStatusCode.Unauthorized, "Brak dostępu", ex.Message),

                System.Security.SecurityException ex =>
                    (HttpStatusCode.Forbidden, "Zabroniona operacja", "Nie masz uprawnień do wykonania tej operacji na tym zasobie."),

                _ => (HttpStatusCode.InternalServerError, "Błąd serwera", "Wystąpił nieoczekiwany błąd.")
            };
        }
    }
}