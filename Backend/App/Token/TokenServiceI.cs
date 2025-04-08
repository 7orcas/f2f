
using Backend.App.Token.Ent;

namespace Backend.App.Token
{
    public interface TokenServiceI
    {
        string CreateToken(TokenValues tokenValues);
        TokenValues DecodeToken(string token);
        void AddToken(string key, string token);
        string? GetToken(string key);
        
    }
}
