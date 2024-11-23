using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using UisceBeatha.Api.Exceptions;

namespace UisceBeatha.Api.Infrastructure
{
    public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) :IExceptionHandler
    {
        private static (int StatusCode, string Type) GetStatusAndType(Exception exception)
        {
            return exception switch
            {
                ConfirmationLinkAlreadySentException => (StatusCodes.Status409Conflict, "https://httpstatuses.com/400"),
                EmailAlreadyConfirmedException => (StatusCodes.Status400BadRequest, "https://httpstatuses.com/400"),
                ArgumentException => (StatusCodes.Status400BadRequest, "https://httpstatuses.com/400"),
                InvalidOperationException => (StatusCodes.Status409Conflict, "https://httpstatuses.com/409"),
                NullReferenceException => (StatusCodes.Status404NotFound, "https://httpstatuses.com/404"),
                _ => (StatusCodes.Status500InternalServerError, "https://httpstatuses.com/500")
            };
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            logger.LogError(exception, exception.Message);

            var (statusCode, errorType) = GetStatusAndType(exception);

            var details = new ProblemDetails
            {
                Title = "An error occurred",
                Status = statusCode,
                Detail = exception.Message,
                Instance = httpContext.Request.Path,
                Type = errorType
            };

            var response = JsonSerializer.Serialize(details);
            httpContext.Response.ContentType = "application/problem+json";
            httpContext.Response.StatusCode = statusCode;
            await httpContext.Response.WriteAsync(response, cancellationToken);

            return true;
        }
    }
}
