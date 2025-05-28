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

        public async Task<AppConfig> GetAppConfig(int userId, int OrgId, string? langCode)
        {
            if (langCode == null) langCode = GC.LangCodeDefault;

            var appConfig = new AppConfig()
            {
                OrgId = OrgId,
                LangCode = langCode,
                IsLabelLink = true
            };

            return appConfig;
        }


    }
}
