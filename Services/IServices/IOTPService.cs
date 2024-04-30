using totp_Module.Data.DTO.TOTPService;

namespace totp_Module.Services.IServices
{
    public interface IOTPService
    {
        Task<string> GenerateSecretKey();
        Task<string> GenerateTOTP(string secretKey);
        Task<bool> VerifyTOTP(string secretKey, string totpKey);
    }
}
