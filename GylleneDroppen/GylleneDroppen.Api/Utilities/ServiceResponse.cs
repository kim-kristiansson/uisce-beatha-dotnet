using Microsoft.AspNetCore.Mvc;

namespace GylleneDroppen.Api.Utilities;

public readonly struct ServiceResponse<T>
{
    public bool IsSuccess { get; init; }
    public T? Data { get; init; }
    public ProblemDetails? ProblemDetails { get; init; }
    public string? RedirectUrl { get; init; }
    
    public static ServiceResponse<T> Success(T data) => new()
    {
        IsSuccess = true,
        Data = data
    };
    
    public static ServiceResponse<T> Failure(string title, string detail, int statusCode) => new()
    {
        IsSuccess = false,
        ProblemDetails = new ProblemDetails
        {
            Title = title,
            Detail = detail,
            Status = statusCode
        }
    };

    public static ServiceResponse<T> Redirect(string redirectUrl) => new()
    {
        RedirectUrl = redirectUrl
    };
}