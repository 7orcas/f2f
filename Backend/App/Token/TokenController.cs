using Microsoft.AspNetCore.Mvc;

namespace Backend.App.Token
{
    [ApiController]
    [Route("api/[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly TokenServiceI _tokenService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="TokenService"></param>
        public TokenController(TokenServiceI tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpGet("token")]
        public IActionResult GetToken([FromQuery] string key)
        {
            var token = _tokenService.GetToken(key);


            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("No token found in session");
            }
            return Ok(token);
        }
    }

}
