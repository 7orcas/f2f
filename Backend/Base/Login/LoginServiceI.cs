
namespace Backend.Base.Login
{
    public interface LoginServiceI
    {
        Task<LoginEnt> GetLogin(string userid);
        Task<bool> IncrementAttempts(LoginEnt l);
        Task<UserEnt> InitialiseLogin(LoginEnt l, OrgEnt org);
        Task<LoginErr> Validate(LoginEnt l, string password, int org);
    }
}
