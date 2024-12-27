using GylleneDroppen.Api.Dtos;
using GylleneDroppen.Api.Utilities;

namespace GylleneDroppen.Api.Services.Interfaces
{
    public interface IAuthService
    {
        Task<ServiceResponse<RegisterResponse>> RegisterAsync(RegisterRequest request);
        Task<ServiceResponse<LoginResponse>> LoginAsync(LoginRequest request);
    }
}
