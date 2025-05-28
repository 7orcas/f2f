using Backend.Base.Config.Ent;

namespace Backend.Base.Config
{
    public interface ConfigServiceI
    {
        Task<AppConfig> GetAppConfig(int userId, int OrgId, string? langCode);
    }
}
