

using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.Caching.Memory;

namespace Backend.Base.Session
{
    public class SessionService: BaseService, SessionServiceI
    {
        private readonly IMemoryCache _memoryCache;

        public SessionService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public async Task<SessionEnt> CreateSession(LoginEnt login, OrgEnt org)
        {
            var key = login.Userid + "-" + Guid.NewGuid().ToString();
            var ses = new SessionEnt
            {
                Key = key,
                Login = login,
                Org = org
            };

            _memoryCache.Set(Key(key), ses);
            return ses;
        }

        public SessionEnt? GetSession(string key)
        {
            if (_memoryCache.TryGetValue(Key(key), out var cachedValue))
            {
                return cachedValue as SessionEnt;
            }
            return null;
        }

        private string Key(string key)
        {
            return "SessionService_" + key;
        }

    }
}
