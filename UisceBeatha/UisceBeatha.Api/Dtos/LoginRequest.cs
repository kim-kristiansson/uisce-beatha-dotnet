﻿using System.ComponentModel.DataAnnotations;

namespace UisceBeatha.Api.Dtos
{
    public class LoginRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; init; } = string.Empty;
        [Required]
        [MinLength(6)]
        public string Password { get; init; } = string.Empty;
    }
}
