
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Caching.Memory;

namespace Backend.App.Token
{
    public class TokenService: BaseService, TokenServiceI
    {

        private readonly IMemoryCache _memoryCache;

        public TokenService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
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
