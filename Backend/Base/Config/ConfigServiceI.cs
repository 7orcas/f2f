using Backend.Base.Config.Ent;

namespace Backend.Base.Config
{
    public interface ConfigServiceI
    {
        UserConfig CreateUserConfig(UserEnt user, OrgEnt org, string? langCode);
    }
}
