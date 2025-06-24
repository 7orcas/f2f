using Newtonsoft.Json;
using GC = FrontendServer.GlobalConstants;

/// <summary>
/// Configuration methods for BasePage
/// Created: June 2025
/// [*Licence*]
/// Author: John Stewart
/// </summary>

namespace FrontendServer.Base._Base
{
    public partial class BasePage
    {
        protected async Task SetConfig()
        {
            if (_config != null) return;
            _config = @ConfigService.Config;
        }

        public async Task<AppConfigDto> GetConfig()
        {
            return _config;
        }

        public void SetConfig(AppConfigDto config)
        {
            _config = config;
            ConfigService.Set(config);
        }


    }
}
