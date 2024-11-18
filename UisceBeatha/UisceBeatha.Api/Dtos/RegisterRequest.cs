using System.ComponentModel.DataAnnotations;

namespace UisceBeatha.Api.Dtos
{
    public class RegisterRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; init; } = string.Empty;
        [Required]
        [MinLength(6)]
        public string Password { get; init; } = string.Empty;
        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; init; } = string.Empty;
    }
}
