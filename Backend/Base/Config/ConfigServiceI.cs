using Backend.Base.Config.Ent;

namespace Backend.Base.Config
{
    public interface ConfigServiceI
    {
        Task<AppConfig> GetAppConfig();
        Task<AppConfig> GetAppConfig(string? langCode, int orgId);
    }
}
