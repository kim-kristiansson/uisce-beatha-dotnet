using WhiskyClub.Api.Dtos;
using WhiskyClub.Api.Models;
using WhiskyClub.Api.Repositories.Interfaces;
using WhiskyClub.Api.Services.Interfaces;

namespace WhiskyClub.Api.Services
{
    public class AuthService(IUserRepository userRepository, IPasswordService passwordService) :IAuthService
    {
        public async Task<UserResponseDto> RegisterAsync(RegisterRequestDto request)
        {
#pragma warning disable CS8602 // Suppress "dereference of a possibly null reference" warnings
            if (await userRepository.IsEmailRegisteredAsync(request.Email))
            {
                throw new InvalidOperationException("Email already registered");
#pragma warning restore CS8602
            }

            var user = new User
            {
                Email = request.Email,
                PasswordHash = passwordService.HashPassword(request.Password)
            };

            await userRepository.AddAsync(user);
            await userRepository.SaveChangesAsync();

            return new UserResponseDto
            {
                Email = user.Email
            };
        }
    }
}
