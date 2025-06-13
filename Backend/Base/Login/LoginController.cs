using Backend.Base.Token.Ent;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Base.Login
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly LoginServiceI _loginService;
        private readonly TokenServiceI _tokenService;
        private readonly OrgServiceI _orgService;
        private readonly ConfigServiceI _configService;
        private readonly SessionServiceI _sessionService;

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
            _tokenService = tokenService;
            _orgService = orgService;
            _configService = configService;
            _sessionService = sessionService;
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {

            var login = await _loginService.GetLogin(request.Username);
            var err = await _loginService.Validate(login, request.Password, request.Org);
            
            if (err != null)
                return Ok(new _ResponseDto
                    {
                        Valid = false,
                        ErrorMessage = err.Message
                    });

            var langCode = !string.IsNullOrEmpty(request.LangCode) ? request.LangCode : login.LangCode;
            var org = await _orgService.GetOrg(request.Org);
            var user = await _loginService.InitialiseLogin(login, org, request.SourceApplication);
            var config = _configService.CreateUserConfig(user, org, langCode);
            var session = await _sessionService.CreateSession(user, org, config, request.SourceApplication);

            var tv = new TokenValues {
                Username = request.Username,
                SessionKey = session.Key,
                Org = request.Org,
            };

            var tokenX = _tokenService.CreateToken(tv);
            var keyX = Guid.NewGuid().ToString();
            _tokenService.AddToken(keyX, tokenX);

            var r = new _ResponseDto
            {
                SuccessMessage = "Login Ok",
                Result = new LoginTokenDto { 
                    TokenKey = keyX,
                    Token = tokenX,
                    MainUrl = AppSettings.MainClientUrl,
                    LangCode = langCode,
                }
            };
            return Ok(r);
        }
    }
}