using GylleneDroppen.Api.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace GylleneDroppen.Api.Extensions;

public static class ServiceResponseExtensions
{
    public static IActionResult ToActionResult<T>(this ServiceResponse<T> serviceResponse)
    {
        if(serviceResponse.RedirectUrl != null)
            return new RedirectResult(serviceResponse.RedirectUrl);

        if (serviceResponse is { IsSuccess: false, ProblemDetails: not null })
            return new ObjectResult(serviceResponse.ProblemDetails)
            {
                StatusCode = serviceResponse.ProblemDetails.Status ?? 500
            };

        if (serviceResponse.IsSuccess)
            return new OkObjectResult(serviceResponse.Data);

        return new StatusCodeResult(500);
    }
}