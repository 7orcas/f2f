using Backend.Base.Config.Ent;
using Backend.Base.Label.Ent;
using Microsoft.Data.SqlClient;
using System.Security.Cryptography;

namespace Backend.Base.Config
{
    public class ConfigService : BaseService, ConfigServiceI
    {
        private AppConfig? _appConfig;

        public ConfigService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public async Task<AppConfig> GetAppConfig()
        {
            if (_appConfig != null) return _appConfig;
            return await GetAppConfig(null, 0);
        }

        public async Task<AppConfig> GetAppConfig(string? langCode, int OrgId)
        {
            if (_appConfig != null) return _appConfig;

            if (langCode == null) langCode = "en";

            _appConfig = new AppConfig()
            {
                OrgId = OrgId,
                LangCode = langCode,
                IsLabelLink = true
            };

            return _appConfig;
        }

    }
}
