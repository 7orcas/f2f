
using Azure.Core;
using Backend.App.Token.Ent;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Backend.App.Token
{
    public class TokenService: BaseService, TokenServiceI
    {
        private readonly IMemoryCache _memoryCache;

        public TokenService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public string CreateToken(TokenValues tv)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, tv.Userid),
                new Claim("Org", "" + tv.Org),
                new Claim("Role", "UserX"),
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TokenParameters._Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: TokenParameters._Issuer,
                audience: TokenParameters._Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public TokenValues DecodeToken(string token)
        {
            token = token.Trim();
            var tokenHandler = new JwtSecurityTokenHandler();

            TokenValues tv = new TokenValues();
            try
            {
                // Validate token and decode
                var principal = tokenHandler.ValidateToken(token, TokenParameters.GetParameters(), out var validatedToken);

                // Extract claims
                tv.Userid = principal.FindFirst(ClaimTypes.Name)?.Value.ToString();
                int.TryParse(principal.FindFirst("Org")?.Value, out int orgId);
                tv.Org = orgId;
            }
            catch (Exception ex)
            {
                //ToDo Logme
                Console.WriteLine($"Token validation failed: {ex.Message}");
            }
            return tv;
        }

        public void AddToken(string key, string token)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5), // Cache expiration
                SlidingExpiration = TimeSpan.FromMinutes(2) // Renew expiration on access
            };

            _memoryCache.Set(Key(key), token, cacheEntryOptions);

        }

        public string? GetToken(string key)
        {
            if (_memoryCache.TryGetValue(Key(key), out var cachedValue))
            {
                return cachedValue.ToString();
            }
            return null;
        }

        private string Key(string key)
        {
            return "TokenService_" + key;
        }
    }
}
