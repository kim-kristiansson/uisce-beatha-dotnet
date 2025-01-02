using System.Text.Json;
using GylleneDroppen.Api.Dtos;
using GylleneDroppen.Api.Models;
using GylleneDroppen.Api.Repositories.Interfaces;
using GylleneDroppen.Api.Services.Interfaces;
using GylleneDroppen.Api.Utilities;

namespace GylleneDroppen.Api.Services
{
    public class AuthService(IUserRepository userRepository, IRedisRepository redisRepository, ISmtpService smtpService) :IAuthService
    {
        public async Task<ServiceResponse<string>> RegisterAsync(RegisterRequest request)
        {
            if (await userRepository.IsEmailRegisteredAsync(request.Email))
            {
                return ServiceResponse<string>.Failure("EmailAlreadyRegistered", 409);
            }

            var tempUserData = new
            {
                Email = request.Email,
                Password = request.Password,
            };
            
            var tempUserJson = JsonSerializer.Serialize(tempUserData);
            
            await redisRepository.SaveAsync($"tempUser:{request.Email}", tempUserJson, TimeSpan.FromMinutes(15));

            var verificationCode = CodeGenerator.GenerateVerificationCode();
            
            await redisRepository.SaveAsync($"verificationCode:{verificationCode}", verificationCode, TimeSpan.FromMinutes(15));
            
            await smtpService.SendEmailAsync(
                "Gyllene Droppen", 
                "NoReply", 
                request.Email, 
                "Verifiera Din E-Post", 
                $"Din verifieringskod är: <strong>{verificationCode}</strong>"
            );

            return ServiceResponse<string>.Success("Verification email sent successfully.");
        }

        public async Task<ServiceResponse<string>> VerifyEmailAsync(VerifyEmailRequest request)
        {
            var storedCode = await redisRepository.GetAsync($"verification:{request.Email}");

            if (storedCode == null || storedCode != request.VerificationCode)
            {
                return ServiceResponse<string>.Failure("InvalidVerificationCoded", 404);
            }
            
            var tempUserJson = await redisRepository.GetAsync($"tempUser:{request.Email}");

            if (tempUserJson == null)
            {
                return ServiceResponse<string>.Failure("UserDataExpired", 400);
            }
            
            var tempUserData = JsonSerializer.Deserialize<dynamic>(tempUserJson);

            var user = new User
            {
                Email = tempUserData.email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(tempUserData.Password)
            };
            
            await userRepository.AddAsync(user);
            await userRepository.SaveChangesAsync();
            
            return ServiceResponse<string>.Success("User created and email verified successfully.");
        }
    }
}
