using Backend.App.Token;
using Backend.App.Token.Ent;
using Backend.Modules._Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Backend.App.Login
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly TokenServiceI _tokenService;
        private readonly LoginServiceI _loginService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="labelService"></param>
        public LoginController(TokenServiceI tokenService,
            LoginServiceI loginService)
        {
            _tokenService = tokenService;
            _loginService = loginService;
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var l = await _loginService.GetLogin(request.Username);

            //ToDo Log this
            if (l.Id == 0)
                return Ok(new _ResponseDto
                {
                    Valid = false,
                    ErrorMessage = "Invalid Username and/or Password."
                });

            await _loginService.IncrementAttempts(l);

            if (!request.Password.Equals(l.Password))
                return Ok(new _ResponseDto
                {
                    Valid = false,
                    ErrorMessage = "Invalid Username and/or Password"
                });

            if (l.Attempts > 3)
                return Ok(new _ResponseDto
                {
                    Valid = false,
                    ErrorMessage = "Max Attempts"
                });

            //ToDo Log this
            var v = _loginService.Validate(l, request.Org);
            if (!string.IsNullOrEmpty(v))
                return Ok(new _ResponseDto
                    {
                        Valid = false,
                        ErrorMessage = v
                    });


            await _loginService.InitialiseLogin(l);

            var tv = new TokenValues { 
                Userid = l.Userid,
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
                    Token = tokenX 
                }
            };
            return Ok(r);
            

        }
    }
}