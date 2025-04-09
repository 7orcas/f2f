
namespace Backend.Base.Session
{
    public interface SessionServiceI
    {
        Task<SessionEnt> CreateSession(LoginEnt login, OrgEnt org);
        SessionEnt? GetSession(string key);
    }
}
