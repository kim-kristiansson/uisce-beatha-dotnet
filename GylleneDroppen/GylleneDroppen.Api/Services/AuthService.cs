using System.Text.Json;
using GylleneDroppen.Api.Dtos;
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
            
            await redisRepository.DeleteAsync($"verification:{request.Email}");
            
            await redisRepository.SaveAsync($"verifiedEmail:{request.Email}", "true", TimeSpan.FromHours(1));

            return ServiceResponse<string>.Success("Email verified successfully.");
        }
    }
}
