using Newtonsoft.Json;
using GC = FrontendServer.GlobalConstants;

/// <summary>
/// Configuration methods for BasePage
/// Created: June 2025
/// [*Licence*]
/// Author: John Stewart
/// </summary>

namespace FrontendServer.Base
{
    public partial class BasePage
    {
        protected async Task SetConfig()
        {
            if (_config != null) return;
            var cjson = await ProtectedSessionStore.GetAsync<string>(GC.ConfigCacheKey);
            _config = JsonConvert.DeserializeObject<AppConfigDto>(cjson.Value);
        }

        public async Task<AppConfigDto> GetConfig()
        {
            return _config;
        }
        
        public string GetLanguageCode()
        {
            if (_config == null) return "?";
            return _config.Label.LangCode;
        }



    }
}
