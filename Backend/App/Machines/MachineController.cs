using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GC = Backend.GlobalConstants;

namespace Backend.App.Machines
{
    [Authorize]
    [PermissionAtt("machine")]
    [ApiController]
    [Route("api/[controller]")]
    public class MachineController : BaseController
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

        [CrudAtt(GC.CrudRead)]
        [HttpGet("list")]
        public async Task<IActionResult> Get()
        {
            var session = HttpContext.Items["session"] as SessionEnt;
            var machines = await _machineService.GetMachines();
            var list = new List<MachineDto>();

            foreach (var m in machines)
            {
                list.Add(new MachineDto
                {
                    Code = m.Code,
                    Description = m.Description + " org desc:" + session.Org.Description,
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