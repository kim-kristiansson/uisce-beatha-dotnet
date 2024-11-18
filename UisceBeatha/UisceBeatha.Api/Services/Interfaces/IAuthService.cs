using UisceBeatha.Api.Dtos;

namespace UisceBeatha.Api.Services.Interfaces
{
    public interface IAuthService
    {
        Task<UserResponse> RegisterAsync(RegisterRequest request);
        Task<UserResponse> LoginAsync(LoginRequest request);
    }
}
