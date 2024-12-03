﻿namespace GylleneDroppen.Api.Services.Interfaces
{
    public interface IPasswordService
    {
        public string HashPassword(string password);
        public bool VerifyPassword(string password, string passwordHash);
    }
}
