using Microsoft.AspNetCore.Mvc;

namespace Rusiru.Portfolio.Api.Middleware;

public sealed class ExceptionHandlingMiddleware
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
        catch (ArgumentException ex)
        {
            await HandleExceptionAsync(context, ex, StatusCodes.Status400BadRequest, "Bad request", ex.Message);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(
                context, 
                ex, 
                StatusCodes.Status500InternalServerError, 
                "Internal server error", "An unexpected error occurred. Please try again later.");
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception, int statusCode, string title, string detail)
    {
        if (context.Response.HasStarted)
        {
            _logger.LogWarning(exception, "The response has already started. The exception handling middleware cannot write an error response.");
            return;
        }

        if (statusCode >= 500)
        {
            _logger.LogError(
                exception,
                "Unhandled exception occurred while processing request {Method} {Path}",
                context.Request.Method,
                context.Request.Path);
        }
        else
        {
            _logger.LogWarning(
                exception,
                "Handled exception occurred while processing request {Method} {Path}",
                context.Request.Method,
                context.Request.Path);
        }

        context.Response.Clear();
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail,
            Instance = context.Request.Path
        };

        problemDetails.Extensions["traceId"] = context.TraceIdentifier;

        await context.Response.WriteAsJsonAsync(problemDetails);
    }
}