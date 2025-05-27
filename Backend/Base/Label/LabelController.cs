using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GC = Backend.GlobalConstants;

namespace Backend.Base.Label
{
    [Authorize]
    [PermissionAtt("label")]
    [ApiController]
    [Route("api/[controller]")]
    public class LabelController : BaseController
    {
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ServiceProvider"></param>
        /// <param name="LabelService"></param>
        public LabelController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        /// <summary>
        /// Gets an appointment
        /// </summary>
        /// <param name="langCode"></param>
        /// <returns></returns>
        [CrudAtt(GC.CrudRead)]
        [AuditListAtt(GC.EntityTypeLangLabel)]
        [HttpGet("list/{code}")]
        public async Task<IActionResult> Get(string code)
        {
            var session = HttpContext.Items["session"] as SessionEnt;
            var labels = await _labelService.GetLabels(code);
            var list = new List<LangLabelDto>();

            foreach (var e in labels)
            {
                list.Add(new LangLabelDto
                {
                    Id = e.Id,
                    LangKeyId = e.LangKeyId,
                    HardCodedNr = e.HardCodedNr,
                    LabelCode = e.LabelCode,
                    Code = e.Code,
                    Tooltip = e.Tooltip,
                    Updated = e.Updated,
                    IsActive = e.IsActive
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