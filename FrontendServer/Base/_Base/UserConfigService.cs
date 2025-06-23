
/// <summary>
/// Scoped user config service
/// Created: June 2025
/// [*Licence*]
/// Author: John Stewart
/// </summary>

namespace FrontendServer.Base._Base
{
    public class UserConfigService
    {
        private AppConfigDto? _config;

        public bool IsInitialized => _config != null;
        public AppConfigDto? Config => _config;

        public void Set(AppConfigDto config) => _config = config;

    }
}
