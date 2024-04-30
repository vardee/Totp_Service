using Microsoft.AspNetCore.Identity;
using System.Data;
using System.Net.Mail;
using totp_Module.Data;
using totp_Module.Data.DTO.AuthService;
using totp_Module.Data.DTO.TOTPService;
using totp_Module.Data.Entities;
using totp_Module.Helpers;
using totp_Module.Services.IServices;

namespace totp_Module.Services
{
    public class AuthService : IAuthService

    {
        private readonly AuthDBContext _authDBContext;
        private readonly IConfiguration _configuration;
        private readonly TokenInteraction _tokenHelper;
        private readonly WeatherService _weatherService;
        private readonly IOTPService _otpService;
        private readonly string _mailDevHost = "localhost";
        private readonly int _mailDevPort = 1025;

        public AuthService(AuthDBContext authDBContext, IConfiguration configuration, TokenInteraction tokenHelper, IOTPService otpService, WeatherService weatherService)
        {
            _authDBContext = authDBContext;
            _configuration = configuration;
            _tokenHelper = tokenHelper;
            _otpService = otpService;
            _weatherService = weatherService;
        }
        public async  Task UserRegister(UserRegisterDTO userRegisterDTO)
        {
            var secretKey = await _otpService.GenerateSecretKey();
            User user = new User()
            {
                Email = userRegisterDTO.Email,
                Password = HashPassword.HashingPassword(userRegisterDTO.Password),
                TOTPSecretKey = secretKey,
            };
            _authDBContext.Users.Add(user);
            await _authDBContext.SaveChangesAsync();
        }

        public async Task<ShowTOTPDTO> UserLogin(UserLoginDTO userLoginDTO)
        {
            var user = _authDBContext.Users.FirstOrDefault(u => u.Email == userLoginDTO.Email);

            if (user == null)
            {
                throw new BadHttpRequestException("Неправильный Email или пароль");
            }
            else if (!HashPassword.VerifyPassword(userLoginDTO.Password, user.Password))
            {
                throw new BadHttpRequestException("Неправильный Email или пароль");
            }
            var totp = await _otpService.GenerateTOTP(user.TOTPSecretKey);

            await SendTOTPEmail(user.Email, totp);

            var secretTOTP = new ShowTOTPDTO
            {
                TOTP = totp,
            };
            return secretTOTP;
        }

        private async Task SendTOTPEmail(string userEmail, string totp)
        {
            try
            {
                using (var message = new MailMessage())
                {
                    message.From = new MailAddress("system@admin.mail");
                    message.To.Add(userEmail);
                    message.Subject = "Попытка входа в аккаунт";
                    message.Body = $"Ваш код для входа в аккаунт {totp}";

                    using (var client = new SmtpClient(_mailDevHost, _mailDevPort))
                    {
                        client.EnableSsl = false;
                        await client.SendMailAsync(message);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new BadHttpRequestException("BadRequest");
            }

        }
        public async Task<UserAuthResponseDTO> AuthVerify(InputTOTPDTO inputTOTPDTO)
        {
            var user = _authDBContext.Users.FirstOrDefault(u => u.Email == inputTOTPDTO.Email);
            if (user == null)
            {
                throw new BadHttpRequestException("ababbababba");
            }

            string expectedOTP = await _otpService.GenerateTOTP(user.TOTPSecretKey);

            bool checkKey = await _otpService.VerifyTOTP(expectedOTP, inputTOTPDTO.TOTPKey);
            if (checkKey)
            {
                return new UserAuthResponseDTO
                {
                    Token = _tokenHelper.GenerateToken(user)
                };
            }
            else
            {
                throw new BadHttpRequestException("ABOBA NEVERNO VOOBSHETO");
            }
        }

    }
}
