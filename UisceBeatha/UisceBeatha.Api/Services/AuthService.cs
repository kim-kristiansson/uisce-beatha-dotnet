using UisceBeatha.Api.Dtos;
using UisceBeatha.Api.Models;
using UisceBeatha.Api.Repositories.Interfaces;
using UisceBeatha.Api.Services.Interfaces;

namespace UisceBeatha.Api.Services
{
    public class AuthService(IUserRepository userRepository, IPasswordService passwordService, IJwtService jwtService) :IAuthService
    {
        public async Task<UserResponse> LoginAsync(LoginRequest request)
        {
            var user = await userRepository.GetUserByEmailAsync(request.Email) ?? throw new InvalidOperationException("Invalid email or password");

            bool isPasswordValid = passwordService.VerifyPassword(request.Password, user.PasswordHash);

            if (!isPasswordValid)
            {
                throw new InvalidOperationException("Invalid email or password");
            }

            return new UserResponse
            {
                Id = user.Id.ToString( ),
                Email = user.Email,
                Token = jwtService.GenerateToken(user)
            };
        }

        public async Task<UserResponse> RegisterAsync(RegisterRequest request)
        {
            if (await userRepository.IsEmailRegisteredAsync(request.Email))
            {
                throw new InvalidOperationException("Email already registered");
            }

            var user = new User
            {
                Email = request.Email,
                PasswordHash = passwordService.HashPassword(request.Password)
            };

            await userRepository.AddAsync(user);
            await userRepository.SaveChangesAsync();

            return new UserResponse
            {
                Id = user.Id.ToString(),
                Email = user.Email,
                Token = jwtService.GenerateToken(user)
            };
        }
    }
}
