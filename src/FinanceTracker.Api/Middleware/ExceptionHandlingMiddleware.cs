using System.Net;
using System.Text.Json;
using FinanceTracker.Application.Common.Exceptions;

namespace FinanceTracker.Api.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, message, errors) = exception switch
        {
            NotFoundException => (HttpStatusCode.NotFound, exception.Message, (object?)null),
            ForbiddenAccessException => (HttpStatusCode.Forbidden, exception.Message, null),
            ValidationException ve => (HttpStatusCode.UnprocessableEntity, "Validation failed.", (object?)ve.Errors),
            UnauthorizedAccessException => (HttpStatusCode.Unauthorized, exception.Message, null),
            InvalidOperationException => (HttpStatusCode.Conflict, exception.Message, null),
            _ => (HttpStatusCode.InternalServerError, "An unexpected error occurred.", null)
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var response = new { status = (int)statusCode, message, errors };
        await context.Response.WriteAsync(JsonSerializer.Serialize(response,
            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));
    }
}
