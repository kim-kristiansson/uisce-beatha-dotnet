using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace GylleneDroppen.Api.Infrastructure
{
    public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) :IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            logger.LogError(exception, exception.Message);

            var details = new ProblemDetails
            {
                Title = "An error occurred",
                Status = StatusCodes.Status500InternalServerError,
                Detail = exception.Message,
                Instance = httpContext.Request.Path,
                Type = exception.GetType().FullName,
            };

            var response = JsonSerializer.Serialize(details);
            httpContext.Response.ContentType = "application/problem+json";
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await httpContext.Response.WriteAsync(response, cancellationToken);

            return true;
        }
    }
}
