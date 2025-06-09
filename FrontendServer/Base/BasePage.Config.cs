using Newtonsoft.Json;
using GC = FrontendServer.GlobalConstants;

namespace FrontendServer.Base
{
    public partial class BasePage
    {
        public async Task SetConfig()
        {
            if (_config != null) return;
            var cjson = await ProtectedSessionStore.GetAsync<string>(GC.ConfigCacheKey);
            _config = JsonConvert.DeserializeObject<AppConfigDto>(cjson.Value);
        }

        public async Task<AppConfigDto> GetConfig()
        {
            await SetConfig();
            return _config;
        }
        
        public string GetLanguageCode()
        {
            SetConfig();
            if (_config == null) return "?";
            return _config.LangCode;
        }



    }
}
