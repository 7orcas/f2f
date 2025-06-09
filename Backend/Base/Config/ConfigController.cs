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
            var appConfig = null as AppConfig;

            if (session.Config != null)
                appConfig = session.Config;
            else
                appConfig = await _ConfigService.GetAppConfig(session.User.LoginId, session.Org.Id, session.Config.LangCode);

            var r = new _ResponseDto
            {
                SuccessMessage = "Config Ok",
                Result = new AppConfigDto
                {
                    IsLabelLink = appConfig.IsLabelLink,
                    OrgId = appConfig.OrgId,
                    OrgDescription = session.Org.Description,
                    LangCode = appConfig.LangCode,
                    UniqueUserId = session.User.LoginId + 987123564,
                    UniqueSessionId = UniqueSessionId.GetId(),

                    Label = new LabelConfigDto
                    {
                        ShowNoKey = true,
                        ShowTooltip = true,
                        ShowLink = true,
                    },
                }
            };

            return Ok(r);
        }


    }
}