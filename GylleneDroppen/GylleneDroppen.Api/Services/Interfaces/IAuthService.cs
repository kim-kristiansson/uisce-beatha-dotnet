using GylleneDroppen.Api.Dtos;

namespace GylleneDroppen.Api.Services.Interfaces
{
    public interface IAuthService
    {
        Task<UserResponse> RegisterAsync(RegisterRequest request);
        Task<UserResponse> LoginAsync(LoginRequest request);
    }
}
