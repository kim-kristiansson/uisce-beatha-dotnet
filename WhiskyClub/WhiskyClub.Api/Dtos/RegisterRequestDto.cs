using System.ComponentModel.DataAnnotations;

namespace WhiskyClub.Api.Dtos
{
    public class RegisterRequestDto
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        [MinLength(6)]
        public string? Password { get; set; }
        [Required]
        [Compare("Password")]
        public string? ConfirmPassword { get; set; }
    }
}
