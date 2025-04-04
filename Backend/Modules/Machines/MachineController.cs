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

        [Authorize]
        [HttpGet("machines")]
        public async Task<IActionResult> Get()
        {
                        
            var machines = await _machineService.GetMachines();
            var list = new List<MachineDto>();

            foreach (var m in machines)
            {
                list.Add(new MachineDto
                {
                    Code = m.Code,
                    Description = m.Description,
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