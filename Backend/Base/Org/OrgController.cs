using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GC = Backend.GlobalConstants;

/// <summary>
/// Organisation controller
/// Created: June 2025
/// [*Licence*]
/// Author: John Stewart
/// </summary>

namespace Backend.Base.Org
{
    [Authorize]
    [PermissionAtt("org")]
    [ApiController]
    [Route("api/[controller]")]
    public class OrgController : BaseController
    {
        private readonly OrgServiceI _orgService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ServiceProvider"></param>
        /// <param name="orgService"></param>
        public OrgController(IServiceProvider serviceProvider,
            OrgServiceI orgService) : base(serviceProvider)
        {
            _orgService = orgService;
        }

        /// <summary>
        /// Get Org
        /// </summary>
        /// <returns></returns>
        [CrudAtt(GC.CrudIgnore)]
        [HttpGet("get")]
        public async Task<IActionResult> GetOrg()
        {
            var session = HttpContext.Items["session"] as SessionEnt;

            var org = await _orgService.GetOrg(session.Org.Nr);
            
            var r = new _ResponseDto
            {
                SuccessMessage = "Config Ok",
                Result = new OrgDto
                {
                    Id = org.Id,
                    Nr = org.Nr,
                    Code = org.Code,
                    Description = org.Description,
                    Updated = org.Updated,
                    IsActive = org.IsActive,
                    LangCode = org.LangCode,
                    LangLabelVariant = org.LangLabelVariant,
                }
            };
            return Ok(r);
        }

        /// <summary>
        /// Update Org
        /// </summary>
        /// <returns></returns>
        [CrudAtt(GC.CrudIgnore)]
        [HttpPost("update")]
        public async Task<IActionResult> UpdateOrg([FromBody] OrgDto dto)
        {
            var session = HttpContext.Items["session"] as SessionEnt;

            var org = new OrgEnt
            {
                Id = dto.Id,
                Nr = dto.Nr,
                Code = dto.Code,
                Description = dto.Description,
                Updated = dto.Updated,
                IsActive = dto.IsActive,
                LangCode = dto.LangCode,
                LangLabelVariant = dto.LangLabelVariant,
            };


            await _orgService.UpdateOrg(org);

            var r = new _ResponseDto
            {
                SuccessMessage = "Save Ok",
                Result = dto
            };
            return Ok(r);
        }

    }
}