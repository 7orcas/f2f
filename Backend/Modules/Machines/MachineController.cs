using Backend.App.Token;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Modules.Machines
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class MachineController : ControllerBase
    {
        private readonly TokenServiceI _tokenService; 
        private readonly MachineServiceI _machineService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="MachineService"></param>
        public MachineController(TokenServiceI tokenService,
            MachineServiceI machineService)
        {
            _tokenService = tokenService;
            _machineService = machineService;
        }

        [HttpGet("machines")]
        public async Task<IActionResult> Get()
        {
            // Access the Authorization header
            var authorizationHeader = Request.Headers["Authorization"].ToString();

            // Extract the token
            var token = " - no token";
            if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer"))
            {
                token = authorizationHeader.Substring("Bearer".Length);
                var tv = _tokenService.DecodeToken(token);
                token = "userid=" + tv.Userid + ", org=" + tv.Org;
            }

            var machines = await _machineService.GetMachines();
            var list = new List<MachineDto>();

            foreach (var m in machines)
            {
                list.Add(new MachineDto
                {
                    Code = m.Code,
                    Description = m.Description + " " + token,
                });
            }

            var r = new _ResponseDto
            {
                SuccessMessage = "Ok",
                Result = list
            };
            return Ok(r);
        }
    }
}