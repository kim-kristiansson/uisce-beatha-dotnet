using System.ComponentModel.DataAnnotations;

namespace GylleneDroppen.Api.Dtos
{
    public class RegisterRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; init; } = string.Empty;
        public string Password { get; init; } = string.Empty;
    }
}
