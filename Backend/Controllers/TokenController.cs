using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TokenController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetToken()
        {
            var session = HttpContext.Session;
            var token = HttpContext.Session.GetString("JwtToken");
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("No token found in session");
            }
            return Ok(token);
        }
    }

}
