using System.Net;
using System.Text.Json;

namespace EmployeeMgmt.API.Middleware
{
    // MNC Architecture Standard: Core custom middleware wrapper for robust logging
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Let the HTTP request pass through safely to your Controllers
                await _next(context);
            }
            catch (Exception ex)
            {
                // If an unexpected crash occurs anywhere down the stream, catch it here!
                await HandleGlobalExceptionAsync(context, ex);
            }
        }

        private async Task HandleGlobalExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError; // Forces a clean 500 status code

            // 1. Log the full details secretly on the server console for debugging
            _logger.LogError(exception, $"An unexpected structural runtime crash occurred: {exception.Message}");

            // 2. Build a uniform corporate error payload template
            var responsePayload = _env.IsDevelopment()
                ? new ErrorDetails(context.Response.StatusCode, exception.Message, exception.StackTrace?.ToString()) // Show stack trace ONLY during local development testing
                : new ErrorDetails(context.Response.StatusCode, "An internal server error occurred. Please contact system support."); // Show a generic safe message in live production systems

            // Serialize into a uniform, readable JSON string block format
            var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var resultJson = JsonSerializer.Serialize(responsePayload, jsonOptions);

            await context.Response.WriteAsync(resultJson);
        }
    }

    // Helper structural class to model uniform error payloads
    public class ErrorDetails
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string? Details { get; set; }

        public ErrorDetails(int statusCode, string message, string? details = null)
        {
            StatusCode = statusCode;
            Message = message;
            Details = details;
        }
    }
}
