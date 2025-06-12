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
        [HttpGet("get")]
        public async Task<IActionResult> GetClientConfig()
        {
            var session = HttpContext.Items["session"] as SessionEnt;
            var appConfig = session.Config;
            var langCodeCurrent = appConfig.LangCode;
            var isAdminLanguage = appConfig.Languages != null;

            //isAdminLanguage = false; //testing
            var langs = new List<LanguageConfigDto>();
            var multiLang = 0;
            for (int i=0; isAdminLanguage && i < appConfig.Languages.Count; i++)
            {
                var l = appConfig.Languages[i];
                if (!l.IsReadable) continue;

                multiLang++;
                langs.Add(new LanguageConfigDto {
                    LangCode = l.LangCode,
                    //Only service or translator permission can update non-default language codes
                    IsUpdateable = l.IsCreateable || (l.IsUpdateable && l.LangCode.Equals(langCodeCurrent))
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
                        LangCode = langCodeCurrent,
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