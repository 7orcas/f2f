using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Modules.Machines
{
    [ApiController]
    [Route("api/[controller]")]
    public class MachineController : ControllerBase
    {
        private readonly MachineServiceI _machineService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="MachineService"></param>
        public MachineController(MachineServiceI machineService)
        {
            _machineService = machineService;
        }

        //[Authorize]
        [HttpGet("machines")]
        public async Task<IActionResult> Get()
        {

            // Access the Authorization header
            var authorizationHeader = Request.Headers["Authorization"].ToString();


            // Extract the token
            var token = " - no token";
            if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
            {
                token = authorizationHeader.Substring("Bearer ".Length);

                // You can now log or inspect the token if necessary
                Console.WriteLine($"Token: {token}");
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