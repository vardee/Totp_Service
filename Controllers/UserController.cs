using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;
using totp_Module.Services.IServices;
using totp_Module.Data.DTO.AuthService;
using totp_Module.Data.DTO.TOTPService;
using Microsoft.AspNetCore.Components.Forms;

namespace totp_Module.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : Controller
    {
        private readonly IAuthService _authService;
        public UserController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost]
        [Route("register")]
        [ProducesResponseType(typeof(Error), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 401)]
        [ProducesResponseType(typeof(Error), 500)]
        public async Task<ActionResult> Register([FromBody] UserRegisterDTO userRegisterDto)
        {
            await _authService.UserRegister(userRegisterDto);
            return Ok();
        }
        [HttpPost]
        [Route("login")]
        [ProducesResponseType(typeof(Error), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 401)]
        [ProducesResponseType(typeof(Error), 500)]
        public async Task<ActionResult<ShowTOTPDTO>> Login([FromBody] UserLoginDTO userLoginDTO)
        {
            var result  = await _authService.UserLogin(userLoginDTO);
            return Ok(result);
        }
        [HttpPost]
        [Route("verify")]
        [ProducesResponseType(typeof(Error), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 401)]
        [ProducesResponseType(typeof(Error), 500)]
        public async Task<ActionResult<UserAuthResponseDTO>> AuthVerify([FromBody] InputTOTPDTO inputTOTPDTO)
        {
            return Ok(await _authService.AuthVerify(inputTOTPDTO));
        }
    }
}
