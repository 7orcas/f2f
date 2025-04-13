
namespace Backend.Base.Session
{
    public interface SessionServiceI
    {
        Task<SessionEnt> CreateSession(UserEnt login, OrgEnt org);
        SessionEnt? GetSession(string key);
    }
}
