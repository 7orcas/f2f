using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LoginApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // Hardcoded credentials for simplicity
            if (request.Username == "user" && request.Password == "password")
            {

                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, request.Username),
                    new Claim("Role", "User")
                };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ThisIsASecureLongEnoughKey1234567890"));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                   issuer: "yourIssuer",
                   audience: "yourAudience",
                   claims: claims,
                   expires: DateTime.Now.AddMinutes(30),
                   signingCredentials: creds
                 );
                var tokenX = new JwtSecurityTokenHandler().WriteToken(token);

                var session = HttpContext.Session;
                HttpContext.Session.SetString("JwtToken", tokenX);

                var r = new _ResponseDto
                {
                    SuccessMessage = "Login Ok",
                    Result = new LoginTokenDto { Token = tokenX }
                };
                return Ok(r);
            }

            var e = new _ResponseDto
            {
                ErrorMessage = "Can't login",
            };

            return Unauthorized(e);
        }
    }
}