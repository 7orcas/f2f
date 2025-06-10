using Backend.Base.Config.Ent;

namespace Backend.Base.Config
{
    public interface ConfigServiceI
    {
        Task<AppConfig> GetAppConfig(UserEnt user, OrgEnt org, string? langCode);
    }
}
