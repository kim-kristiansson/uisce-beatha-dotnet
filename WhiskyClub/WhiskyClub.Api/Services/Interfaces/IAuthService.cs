using WhiskyClub.Api.Dtos;

namespace WhiskyClub.Api.Services.Interfaces
{
    public interface IAuthService
    {
        Task<UserResponseDto> RegisterAsync(RegisterRequestDto request);
    }
}
