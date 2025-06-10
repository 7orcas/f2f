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
        /// Gets the passed in langCode's labels
        /// The returned DTO objects contain minimal data
        /// </summary>
        /// <param name="langCode"></param>
        /// <returns></returns>
        [CrudAtt(GC.CrudRead)]
        [AuditListAtt(GC.EntityTypeLangLabelList)]
        [HttpGet("clientlist/{langCode}")]
        public async Task<IActionResult> GetLabelList(string langCode)
        {
            var session = HttpContext.Items["session"] as SessionEnt;
            var labels = await _labelService.GetLanguageLabelList(langCode);
            var list = new List<LangLabelDto>();

            foreach (var l in labels)
            {
                list.Add(new LangLabelDto
                {
                    Id = l.Id,
                    LangKeyCode = l.LangKeyCode,
                    Label = l.Code,
                    Tooltip = l.Tooltip
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
        /// Gets related language values the passed in label key code
        /// </summary>
        /// <param name="LangKeyCode"></param>
        /// <returns></returns>
        [CrudAtt(GC.CrudRead)]
        [AuditListAtt(GC.EntityTypeLangLabelRelated)]
        [HttpGet("relatedlist/{LangKeyCode}")]
        public async Task<IActionResult> GetRelatedLabels(string langKeyCode)
        {
            var session = HttpContext.Items["session"] as SessionEnt;
            var langs = session.Config.Languages;
            var langCodes = new List<string>();
            foreach (var lc in langs)
                if (lc.IsReadable)
                    langCodes.Add(lc.LangCode);
            
            if (langCodes.Count == 0)
                return Ok(new _ResponseDto
                    {
                        Valid = false,
                        StatusCode = 401,
                        ErrorMessage = "Language admin not configured",
                    });
            langCodes.Sort();

            var key = await _labelService.GetLanguageKey(langKeyCode);
            var labels = await _labelService.GetRelatedLabels(langKeyCode, langCodes);
            var list = new List<LangLabelDto>();

            foreach (var lc in langCodes)
            {
                var dto = null as LangLabelDto;
                foreach (var l in labels)
                    if (l.LangCode == lc)
                    {
                        dto = new LangLabelDto
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
                        break;
                    }

                if (dto == null)
                    dto = new LangLabelDto
                    {
                        Id = GC.NewRecordId,
                        LangKeyId = key.Id,
                        LangCode = lc,
                    };
                list.Add(dto);
            }


            //Put default language at top
            var listX = new List<LangLabelDto>();
            listX.Add(list.Find(l => l.LangCode == session.Config.LangCode));

            foreach (var l in list)
            {
                if (l.LangCode != session.Config.LangCode)
                    listX.Add(l);
            }


            var r = new _ResponseDto
            {
                SuccessMessage = "Ok",
                Result = listX
            };
            return Ok(r);
        }


    }
}