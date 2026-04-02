using System.Net;
using System.Text.Json;

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
            _logger.LogError(ex, "An unhandled exception occurred.");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var code = HttpStatusCode.InternalServerError; // Default to 500
        var message = "An internal server error occurred.";

        if (exception is HttpRequestException httpEx)
        {
            code = httpEx.StatusCode ?? HttpStatusCode.BadGateway;
            message = "External API communication error.";
        }
        else if (exception is JsonException)
        {
            code = HttpStatusCode.UnprocessableEntity;
            message = "The data returned from the provider was malformed.";
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;

        var result = JsonSerializer.Serialize(new { error = message, details = exception.Message });
        return context.Response.WriteAsync(result);
    }
}