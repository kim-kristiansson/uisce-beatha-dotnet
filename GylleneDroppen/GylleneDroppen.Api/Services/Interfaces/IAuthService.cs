﻿using GylleneDroppen.Api.Dtos;
using GylleneDroppen.Api.Utilities;

namespace GylleneDroppen.Api.Services.Interfaces
{
    public interface IAuthService
    {
        Task<ServiceResponse<string>> RegisterAsync(RegisterRequest request);
        Task<ServiceResponse<string>> VerifyEmailAsync(VerifyEmailRequest request);
    }
}
