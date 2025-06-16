using Backend.Base.Token.Ent;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Base.Login
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly LoginServiceI _loginService;
        

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="labelService"></param>
        public LoginController(
            LoginServiceI loginService,
            TokenServiceI tokenService,
            OrgServiceI orgService,
            ConfigServiceI configService,
            SessionServiceI sessionService)
        {
            _loginService = loginService;
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var login = await _loginService.LoginUser(request.Username, request.Password, request.Org, request.SourceApplication, request.LangCode);
            var res = login.Response;

            if (!res.Valid)
                return Ok(new _ResponseDto
                    {
                        Valid = false,
                        ErrorMessage = res.ErrorMessage,
                    });

            var r = new _ResponseDto
            {
                SuccessMessage = "Login Ok",
                Result = new LoginTokenDto { 
                    TokenKey = res.TokenKey,
                    Token = res.Token,
                    MainUrl = res.MainUrl,
                    LangCode = res.LangCode,
                }
            };
            return Ok(r);
        }
    }
}