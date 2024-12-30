using GylleneDroppen.Api.Dtos;
using GylleneDroppen.Api.Repositories.Interfaces;
using GylleneDroppen.Api.Services.Interfaces;
using GylleneDroppen.Api.Utilities;
using Newtonsoft.Json;
using Stripe;

namespace GylleneDroppen.Api.Services
{
    public class AuthService(IUserRepository userRepository, IPasswordService passwordService, IJwtService jwtService, IRedisRepository redisRepository, IStripeService stripeService) :IAuthService
    {
        public async Task<ServiceResponse<LoginResponse>> LoginAsync(LoginRequest request)
        {
            var user = await userRepository.GetUserByEmailAsync(request.Email) ?? throw new InvalidOperationException("Invalid email or password");

            var isPasswordValid = passwordService.VerifyPassword(request.Password, user.PasswordHash);

            if (!isPasswordValid)
            {
                return ServiceResponse<LoginResponse>.Failure("InvalidCredentials", 401);
            }

            var response = new LoginResponse
            {
                Id = user.Id.ToString( ),
                Email = user.Email,
                Token = jwtService.GenerateToken(user)
            };
            
            return ServiceResponse<LoginResponse>.Success(response);
        }

        public async Task<ServiceResponse<RegisterResponse>> RegisterAsync(RegisterRequest request)
        {
            if (await userRepository.IsEmailRegisteredAsync(request.Email))
            {
                return ServiceResponse<RegisterResponse>.Failure("EmailAlreadyRegistered", 409);
            }

            var tempUserData = new
            {
                request.Email,
                PasswordHash = passwordService.HashPassword(request.Password),
                request.Firstname,
                request.Lastname,
                request.City,
                request.PostalCode,
                request.StreetAddress
            };
            
            const string successUrl = "https://yourapp.com/success?session_id={CHECKOUT_SESSION_ID}";
            const string cancelUrl = "https://yourapp.com/cancel";

            var stripeSessionId = await stripeService.CreateCheckoutSessionAsync(
                request.Email,
                request.Email,
                successUrl,
                cancelUrl
            );

            return ServiceResponse<RegisterResponse>.Success(new RegisterResponse
            {
                Email = request.Email,
                StripeSessionId = stripeSessionId
            });
        }

        public async Task<ServiceResponse<string>> VerifyEmailAsync(VerifyEmailRequest request)
        {
            var redisKey = $"verification:{request.Email}";
            var storedCode = await redisRepository.GetAsync(redisKey);

            if (storedCode == null)
            {
                return ServiceResponse<string>.Failure("VerificationCodeExpired", 400);
            }

            if (storedCode != request.VerificationCode)
            {
                return ServiceResponse<string>.Failure("InvalidVerificationCode", 400);
            }
            
            await redisRepository.DeleteAsync(redisKey);
            
            return ServiceResponse<string>.Success("Email verified successfully.");
        }
    }
}
