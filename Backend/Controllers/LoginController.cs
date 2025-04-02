using Microsoft.AspNetCore.Mvc;

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
                var r = new _ResponseDto
                {
                    SuccessMessage = "Login Ok",
                    Result = new LoginTokenDto { Token = "dummy-jwt-token" }
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