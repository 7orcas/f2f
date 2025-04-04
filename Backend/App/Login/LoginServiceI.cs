using Backend.App.Login.Ent;

namespace Backend.App.Login
{
    public interface LoginServiceI
    {
        Task<Backend.App.Login.Ent.Login> GetLogin(string userid);
        Task<bool> IncrementAttempts(Backend.App.Login.Ent.Login l);
        Task<bool> InitialiseLogin(Backend.App.Login.Ent.Login l);

        string? Validate(Backend.App.Login.Ent.Login l, int org);
    }
}
