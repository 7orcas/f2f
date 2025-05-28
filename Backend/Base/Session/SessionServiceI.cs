
namespace Backend.Base.Session
{
    public interface SessionServiceI
    {
        Task<SessionEnt> CreateSession(UserEnt login, OrgEnt org, AppConfig config, int sourceApp);
        Task RemoveSession(string key);
        SessionEnt? GetSession(string key);
    }
}
