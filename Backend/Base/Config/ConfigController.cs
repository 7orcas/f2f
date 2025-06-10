using Backend.App.Machines;
using Backend.Base.Label.Ent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GC = Backend.GlobalConstants;

namespace Backend.Base.Config
{
    [Authorize]
    [PermissionAtt("config")]
    [ApiController]
    [Route("api/[controller]")]
    public class ConfigController : BaseController
    {
        private readonly ConfigServiceI _ConfigService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ServiceProvider"></param>
        /// <param name="ConfigService"></param>
        public ConfigController(IServiceProvider serviceProvider,
            ConfigServiceI ConfigService) : base(serviceProvider)
        {
            _ConfigService = ConfigService;
        }

        [CrudAtt(GC.CrudIgnore)]
        [HttpGet("get")]
        public async Task<IActionResult> Get()
        {
            var session = HttpContext.Items["session"] as SessionEnt;
            var appConfig = session.Config;

            var langs = new List<LanguageConfigDto>();
            var multiLang = 0;
            for (int i=0; appConfig.Languages != null && i < appConfig.Languages.Count; i++)
            {
                var l = appConfig.Languages[i];
                multiLang += l.IsReadable ? 1 : 0;
                langs.Add(new LanguageConfigDto {
                    LangCode = l.LangCode,
                    IsCreateable = l.IsCreateable,
                    IsReadable = l.IsReadable,
                    IsUpdateable = l.IsUpdateable,
                });
            }
            
            var r = new _ResponseDto
            {
                SuccessMessage = "Config Ok",
                Result = new AppConfigDto
                {
                    OrgId = appConfig.OrgId,
                    OrgDescription = session.Org.Description,
                    UniqueUserId = session.User.LoginId + 987123564,
                    UniqueSessionId = UniqueSessionId.GetId(),

                    Languages = langs.ToArray(),
                    Label = new LabelConfigDto
                    {
                        LangCode = appConfig.LangCode,
                        ShowNoKey = true,
                        ShowTooltip = true,
                        ShowLink = langs.Count > 0,
                        IsMultiLangView = multiLang > 0,
                    },
                }
            };

            return Ok(r);
        }


    }
}