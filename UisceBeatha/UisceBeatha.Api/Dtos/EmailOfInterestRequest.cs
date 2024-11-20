using System.ComponentModel.DataAnnotations;

namespace UisceBeatha.Api.Dtos;

public class EmailOfInterestRequest
{
    [Required]
    [EmailAddress]
    public string EmailAddress { get; init; } = string.Empty;
}