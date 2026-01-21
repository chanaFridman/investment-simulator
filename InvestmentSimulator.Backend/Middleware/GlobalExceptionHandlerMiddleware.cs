using System.Net;
using System.Text.Json;

namespace InvestmentSimulator.Backend.Middleware;

public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

    public GlobalExceptionHandlerMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionHandlerMiddleware> logger)
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
        catch (Exception ex) when (ex is InvalidOperationException || ex is ArgumentException)
        {
            _logger.LogWarning(ex, "Validation error: {Message}", ex.Message);
            await WriteError(context, HttpStatusCode.BadRequest, "Validation Error", ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");
            await WriteError(context, HttpStatusCode.InternalServerError, "Server Error", "An unexpected error occurred.");
        }
    }

    private static async Task WriteError(
        HttpContext context,
        HttpStatusCode statusCode,
        string title,
        string detail)
    {
        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/json";

        var problem = new
        {
            status = (int)statusCode,
            title = title,
            detail = detail,
            traceId = context.TraceIdentifier
        };

        var response = JsonSerializer.Serialize(problem);
        await context.Response.WriteAsync(response);
    }
}