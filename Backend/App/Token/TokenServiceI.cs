
namespace Backend.App.Token
{
    public interface TokenServiceI
    {
        void AddToken(string key, string token);
        string? GetToken(string key);
        
    }
}
