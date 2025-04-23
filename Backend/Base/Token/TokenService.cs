
using Azure.Core;
using Backend.Base.Token.Ent;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Backend.Base.Token
{
    public class TokenService: BaseService, TokenServiceI
    {
        private readonly IMemoryCache _memoryCache;

        public TokenService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        /// <summary>
        /// Login process creates this token
        /// </summary>
        /// <param name="tv"></param>
        /// <returns></returns>
        public string CreateToken(TokenValues tv)
        {
Console.WriteLine("Creating token");
            var claims = new[]
            {
                new Claim("Key", tv.SessionKey),
                new Claim("Org", "" + tv.Org),
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TokenParameters._Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: TokenParameters._Issuer,
                audience: TokenParameters._Audience,
                claims: claims,
                expires: DateTime.Now.AddHours(30),
                signingCredentials: creds
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Every call will use this method
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
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
                tv.SessionKey = principal.FindFirst("Key")?.Value.ToString();
                int.TryParse(principal.FindFirst("Org")?.Value, out int orgId);
                tv.Org = orgId;
Console.WriteLine("Decode token, SessionKey=" + tv.SessionKey);
                return tv;
            }
            catch (Exception ex)
            {
                //ToDo Logme
                Console.WriteLine($"Token validation failed: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Login process adds this token
        /// </summary>
        /// <param name="key"></param>
        /// <param name="token"></param>
        public void AddToken(string key, string token)
        {
Console.WriteLine("Add token, key=" + key);
            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30) // Cache expiration
            };

            _memoryCache.Set(Key(key), token, cacheEntryOptions);

        }

        /// <summary>
        /// Login process will call this method twice
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string? GetToken(string key)
        {
Console.WriteLine("Get token, key=" + key);
            if (_memoryCache.TryGetValue(Key(key), out var cachedValue))
            {
               // _memoryCache.Remove(Key(key));
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
