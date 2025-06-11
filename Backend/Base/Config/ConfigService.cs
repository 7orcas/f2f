using Backend.Base.Config.Ent;
using Backend.Base.Label.Ent;
using Microsoft.Data.SqlClient;
using System.Security.Cryptography;
using GC = Backend.GlobalConstants;

namespace Backend.Base.Config
{
    public class ConfigService : BaseService, ConfigServiceI
    {
        public ConfigService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public async Task<AppConfig> GetAppConfig(UserEnt user, OrgEnt org, string? langCode)
        {
            if (langCode == null) langCode = GC.LangCodeDefault;

            var appConfig = new AppConfig()
            {
                OrgId = org.Id,
                LangCode = langCode,
            };

            if (user.IsAdminLanguage || user.IsService)
                appConfig.Languages = await GetClientLanguages(user, org, langCode);

            return appConfig;
        }


        /**
        * Get list of language codes the client is able to view / edit
        * ToDo: logic for flags
        */
        private async Task<List<LanguageConfig>> GetClientLanguages(UserEnt user, OrgEnt org, string langCode)
        {
            var langList = new List<LanguageConfig>();
            
            await Sql.Run(
                    "SELECT * " +
                    "FROM base.langCode " +
                    "WHERE" + TestActive(),
                    r => {
                        var l = new LanguageConfig();
                        l.LangCode = GetCode(r);
                        langList.Add(l);
                    }
                );

            foreach (var l in langList)
            {
                l.IsCreateable = user.IsService;
                l.IsReadable = true;
                l.IsUpdateable = user.IsService || l.LangCode.Equals(langCode);
            }

            return langList;
        }

    }
}
