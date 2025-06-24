/// <summary>
/// Scoped config service
/// Created: June 2025
/// [*Licence*]
/// Author: John Stewart
/// </summary>
namespace FrontendServer.Base.Config
{
    public class ConfigService
    {
        private AppConfigDto? config;

        public bool IsInitialized => config != null;
        public AppConfigDto? Config => config;
        public bool IsDebugMode => config != null && config.DebugMode;

        public void Set(AppConfigDto config) => this.config = config;
    }
}
