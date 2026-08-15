using System.Net;
using Microsoft.EntityFrameworkCore;
using TrainingCenter.Api.Common;

namespace TrainingCenter.Api.Middleware;

public class GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Database update failed.");
            await WriteErrorAsync(context, HttpStatusCode.Conflict,
                "A database conflict occurred. The operation could not be completed.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception.");
            var message = context.RequestServices.GetRequiredService<IHostEnvironment>().IsDevelopment()
                    || context.RequestServices.GetRequiredService<IHostEnvironment>().IsEnvironment("Testing")
                ? ex.Message
                : "An unexpected error occurred.";
            await WriteErrorAsync(context, HttpStatusCode.InternalServerError, message);
        }
    }

    private static async Task WriteErrorAsync(HttpContext context, HttpStatusCode statusCode, string message)
    {
        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(ApiResponse<object>.Fail(message));
    }
}
