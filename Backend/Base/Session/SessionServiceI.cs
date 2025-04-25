
namespace Backend.Base.Session
{
    public interface SessionServiceI
    {
        Task<SessionEnt> CreateSession(UserEnt login, OrgEnt org, int sourceApp);
        SessionEnt? GetSession(string key);
    }
}
