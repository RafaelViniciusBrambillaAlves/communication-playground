using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RabbitMQService.Domain.Exceptions;

namespace RabbitMQService.Worker.Middleware;

public sealed class ExceptionMiddleware(
    RequestDelegate next,
    ILogger<ExceptionMiddleware> logger
)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (UserNotFoundException ex)
        {
            logger.LogWarning(ex, "Resource not found: {Message}", ex.Message);
            await WriteProblemAsync(context, StatusCodes.Status404NotFound, "Not Found", ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Unhandled exception.");
            await WriteProblemAsync(
                context, 
                StatusCodes.Status500InternalServerError, 
                "Internal Server Error",
                "An unexpected error occurred.");
        }
    }

    private static Task WriteProblemAsync(
        HttpContext context, int status, string title, string detail)
    {
        context.Response.StatusCode = status;
        context.Response.ContentType = "application/problem+json";

        return context.Response.WriteAsJsonAsync(new ProblemDetails
        {
           Status = status,
           Title = title,
           Detail = detail, 
        });
    }
}
