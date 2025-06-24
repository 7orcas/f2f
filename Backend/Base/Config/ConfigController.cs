using Backend.App.Machines;
using Backend.Base.Label.Ent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GC = Backend.GlobalConstants;

/// <summary>
/// Configuration is a combination of the org config and the user settings.
/// It can be used by the client to configuration options and functionality.
/// Created: May 2025
/// [*Licence*]
/// Author: John Stewart
/// </summary>

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

        /// <summary>
        /// Get client config
        /// </summary>
        /// <returns></returns>
        [CrudAtt(GC.CrudIgnore)]
        [HttpGet("clientConfig")]
        public async Task<IActionResult> GetClientConfig()
        {
            var session = HttpContext.Items["session"] as SessionEnt;
            var org = session.Org;
            var user = session.User;
            var userConfig = session.UserConfig;

            var langCodeCurrent = userConfig.LangCodeCurrent;
            var isAdminLanguage = false; // userConfig.Languages != null;

            var langs = new List<LanguageConfigDto>();
            foreach (var lang in userConfig.Languages)
            {
                langs.Add(new LanguageConfigDto {
                    LangCode = lang.LangCode,
                    IsUpdateable = lang.IsEditable,
                });
            }
            
            var r = new _ResponseDto
            {
                SuccessMessage = "Config Ok",
                Result = new AppConfigDto
                {
                    OrgId = userConfig.OrgId,
                    OrgDescription = session.Org.Description,
                    UniqueUserId = session.User.UserAccountId + 987123564,
                    UniqueSessionId = UniqueSessionId.GetId(),

                    Languages = langs.ToArray(),
                    Label = new LabelConfigDto
                    {
                        LangCode = langCodeCurrent,
                        Variant = org.LangLabelVariant,
                        ShowTooltip = true,
                        HighlightNoKey = isAdminLanguage, //ToDo turn on/off
                        IsAdminLanguage = isAdminLanguage, //ToDo turn on/off
                    },
                }
            };
            return Ok(r);
        }
    }
}