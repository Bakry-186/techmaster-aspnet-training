namespace TrainingCenter.Api.Middleware;

public class RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var requestId = context.TraceIdentifier;
        logger.LogInformation(
            "HTTP {Method} {Path} started (RequestId: {RequestId})",
            context.Request.Method,
            context.Request.Path,
            requestId);

        await next(context);

        logger.LogInformation(
            "HTTP {Method} {Path} completed with {StatusCode} (RequestId: {RequestId})",
            context.Request.Method,
            context.Request.Path,
            context.Response.StatusCode,
            requestId);
    }
}
