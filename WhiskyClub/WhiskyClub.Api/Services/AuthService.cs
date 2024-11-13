using WhiskyClub.Api.Dtos;
using WhiskyClub.Api.Models;
using WhiskyClub.Api.Repositories.Interfaces;
using WhiskyClub.Api.Services.Interfaces;

namespace WhiskyClub.Api.Services
{
    public class AuthService(IUserRepository userRepository, IPasswordService passwordService) :IAuthService
    {
        public async Task<UserResponseDto> LoginAsync(LoginRequestDto request)
        {
            var user = await userRepository.GetUserByEmailAsync(request.Email!);

            if (user == null)
            {
                throw new InvalidOperationException("Invalid email or password");
            }

            bool isPasswordValid = passwordService.VerifyPassword(request.Password!, user.PasswordHash!);

            if (!isPasswordValid)
            {
                throw new InvalidOperationException("Invalid email or password");
            }

            return new UserResponseDto
            {
                Email = user.Email
            };
        }

        public async Task<UserResponseDto> RegisterAsync(RegisterRequestDto request)
        {
            if (await userRepository.IsEmailRegisteredAsync(request.Email!))
            {
                throw new InvalidOperationException("Email already registered");
            }

            var user = new User
            {
                Email = request.Email,
                PasswordHash = passwordService.HashPassword(request.Password!)
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
