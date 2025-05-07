using MonthlyExpenseTracker.Helper.ResponseVM;
using System.Net;

namespace MonthlyExpenseTracker.Helper.Middleware;

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
            // Call the next middleware in the pipeline
            await _next(context);
        }
        catch (Exception ex)
        {
            // Log the exception for troubleshooting
            _logger.LogError(ex, "An unexpected error occurred.");

            // Handle the exception and return a standardized error response
            var responseVm = new ResponseVm<string>(
                new List<string> { ex.Message },
                "An unexpected error occurred.",
                (int)HttpStatusCode.InternalServerError
            );

            // Set the response status code
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            // Write the response as a JSON object
            await context.Response.WriteAsJsonAsync(responseVm);
        }
    }

}
