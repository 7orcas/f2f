﻿using Backend.App.Token;
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
        private readonly LoginServiceI _loginService;
        private readonly TokenServiceI _tokenService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="labelService"></param>
        public LoginController(LoginServiceI loginService,
            TokenServiceI tokenService)
        {
            _loginService = loginService;
            _tokenService = tokenService;
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



            var claims = new[]
            {
                new Claim(ClaimTypes.Name, l.Userid),
                new Claim("Org", "" + request.Org),
                new Claim("Role", "User"),
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ThisIsASecureLongEnoughKeyZ1234567890"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "yourIssuer",
                audience: "yourAudience",
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
                );
            var tokenX = new JwtSecurityTokenHandler().WriteToken(token);

            var keyX = Guid.NewGuid().ToString();
            _tokenService.AddToken(keyX, tokenX);

            //var session = HttpContext.Session;
            //HttpContext.Session.SetString("JwtToken", tokenX);
            //HttpContext.Session.SetString("TokenKey", keyX);

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