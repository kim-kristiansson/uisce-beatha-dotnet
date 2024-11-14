using UisceBeatha.Api.Dtos;

namespace UisceBeatha.Api.Services.Interfaces
{
    public interface IAuthService
    {
        Task<UserResponseDto> RegisterAsync(RegisterRequestDto request);
        Task<UserResponseDto> LoginAsync(LoginRequestDto request);
    }
}
