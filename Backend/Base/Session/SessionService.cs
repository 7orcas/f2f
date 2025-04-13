

using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.Caching.Memory;

namespace Backend.Base.Session
{
    /// <summary>
    /// Create and manage sessions containing relevant objects for the logged in user
    /// Note a user can have multiple sessions open
    /// </summary>
    /// <author>John Stewart</author>
    /// <created>April 5, 2025</created>
    /// <license>**Licence**</license>
    public class SessionService: BaseService, SessionServiceI
    {
        private readonly IMemoryCache _memoryCache;

        public SessionService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public async Task<SessionEnt> CreateSession(UserEnt user, OrgEnt org)
        {
            var key = user.Userid + "-" + Guid.NewGuid().ToString();
            var ses = new SessionEnt
            {
                Key = key,
                User = user,
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
