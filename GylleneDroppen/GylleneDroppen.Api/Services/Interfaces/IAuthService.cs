using GylleneDroppen.Api.Dtos;
using GylleneDroppen.Api.Utilities;

namespace GylleneDroppen.Api.Services.Interfaces
{
    public interface IAuthService
    {
        Task<ServiceResponse<UserResponse>> RegisterAsync(RegisterRequest request);
        Task<ServiceResponse<UserResponse>> LoginAsync(LoginRequest request);
    }
}
