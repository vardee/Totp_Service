using totp_Module.Data.DTO.AuthService;
using totp_Module.Data.DTO.TOTPService;

namespace totp_Module.Services.IServices
{
    public interface IAuthService
    {
        Task UserRegister(UserRegisterDTO userRegisterDTO);
        Task<ShowTOTPDTO> UserLogin(UserLoginDTO userLoginDTO);
        Task<UserAuthResponseDTO> AuthVerify(InputTOTPDTO inputTOTPDTO);
    }
}
