using GylleneDroppen.Api.Dtos;
using GylleneDroppen.Api.Models;
using GylleneDroppen.Api.Repositories.Interfaces;
using GylleneDroppen.Api.Services.Interfaces;

namespace GylleneDroppen.Api.Services
{
    public class AuthService(IUserRepository userRepository, IPasswordService passwordService, IJwtService jwtService) :IAuthService
    {
        public async Task<UserResponse> LoginAsync(LoginRequest request)
        {
            var user = await userRepository.GetUserByEmailAsync(request.Email) ?? throw new InvalidOperationException("Invalid email or password");

            var isPasswordValid = passwordService.VerifyPassword(request.Password, user.PasswordHash);

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
