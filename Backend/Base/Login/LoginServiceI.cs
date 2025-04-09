
namespace Backend.Base.Login
{
    public interface LoginServiceI
    {
        Task<LoginEnt> GetLogin(string userid);
        Task<bool> IncrementAttempts(LoginEnt l);
        Task<bool> InitialiseLogin(LoginEnt l);
        Task<LoginErr> Validate(LoginEnt l, string password, int org);
    }
}
