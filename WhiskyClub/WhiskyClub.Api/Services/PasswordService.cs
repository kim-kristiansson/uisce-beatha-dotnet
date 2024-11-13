﻿using WhiskyClub.Api.Services.Interfaces;

namespace WhiskyClub.Api.Services
{
    public class PasswordService :IPasswordService
    {
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
        public bool VerifyPassword(string password, string passwordHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }
    }
}
