
using Azure.Core;
using Backend.Base.Token.Ent;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Backend.Base.Token
{
    /// <summary>
    /// Tokens are used to control authorisation to this app
    /// </summary>
    public class TokenService: BaseService, TokenServiceI
    {
        private readonly IMemoryCache _memoryCache;
        private const int PAD_TOKEN = 5;

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

            _log.Debug("Create token, TokenValues=" + tv.ToLogString());

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
                _log.Debug("Decode token, SessionKey=" + tv.SessionKey);
                return tv;
            }
            catch (Exception ex)
            {
                _log.Error("Token validation failed: {ex.Message}");
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
            string tokenX = AppSettings.MaxGetTokenCalls.ToString().PadLeft(PAD_TOKEN, '0') + token;
            _log.Debug("Add token, key=" + key + ", token=" + tokenX);

            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(AppSettings.CacheExpirationAddSeconds) // Cache expiration
            };
            _memoryCache.Set(Key(key), tokenX, cacheEntryOptions);
        }

        /// <summary>
        /// Login process can call this method twice
        /// The number of calls is limited by appsetting value
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string? GetToken(string key)
        {
            _log.Debug("GetToken, key=" + key);

            if (_memoryCache.TryGetValue(Key(key), out var cachedValue))
            {
                var tokenX = cachedValue.ToString();
                var calls = int.Parse(tokenX.Substring(0, PAD_TOKEN));
                var token = tokenX.Substring(PAD_TOKEN);
                
                _memoryCache.Remove(Key(key));

                if (--calls >= 0)
                {
                    var cacheEntryOptions = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(AppSettings.CacheExpirationGetSeconds) // Cache expiration
                    };
                    tokenX = calls.ToString().PadLeft(PAD_TOKEN, '0') + token;
                    _memoryCache.Set(Key(key), tokenX, cacheEntryOptions);
                    return token;
                }
                _log.Error("GetToken rejected, calls=" + calls + ", key = " + key);
            }
            _log.Error("GetToken is null, key=" + key);
            return null;
        }

        private string Key(string key)
        {
            return "TokenService_" + key;
        }
    }
}
