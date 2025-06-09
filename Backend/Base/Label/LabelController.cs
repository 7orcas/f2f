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
        /// Gets the passed in langKeyCode's labels
        /// The returned DTO objects contain minimal data
        /// </summary>
        /// <param name="langCode"></param>
        /// <returns></returns>
        [CrudAtt(GC.CrudRead)]
        [AuditListAtt(GC.EntityTypeLangLabel)]
        [HttpGet("clientlist/{langKeyCode}")]
        public async Task<IActionResult> Get(string langKeyCode)
        {
            var session = HttpContext.Items["session"] as SessionEnt;
            var labels = await _labelService.GetLanguageLabelList(langKeyCode);
            var list = new List<LangLabelDto>();

            foreach (var e in labels)
            {
                list.Add(new LangLabelDto
                {
                    Id = e.Id,
                    LangKeyCode = e.LangKeyCode,
                    Label = e.Code,
                    Tooltip = e.Tooltip
                });
            }

            var r = new _ResponseDto
            {
                SuccessMessage = "Ok",
                Result = list
            };
            return Ok(r);
        }

        /// <summary>
        /// Gets the passed in langId's label
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [CrudAtt(GC.CrudRead)]
        [AuditListAtt(GC.EntityTypeLangLabel)]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var session = HttpContext.Items["session"] as SessionEnt;
            var l = await _labelService.GetLanguageLabel(id);

            var dto = new LangLabelDto
            {
                Id = l.Id,
                LangKeyId = l.LangKeyId,
                HardCodedNr = l.HardCodedNr,
                LangCode = l.LangCode,
                Label = l.Code,
                Tooltip = l.Tooltip,
                Updated = l.Updated,
                IsActive = l.IsActive
            };

            var r = new _ResponseDto
            {
                SuccessMessage = "Ok",
                Result = dto
            };
            return Ok(r);
        }


    }
}