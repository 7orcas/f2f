using GC = Backend.GlobalConstants;
using Microsoft.Extensions.Caching.Memory;

/// <summary>
/// Create and manage sessions containing relevant objects for the logged in user
/// Note a user can have multiple sessions open
/// Created: April 2025
/// [*Licence*]
/// Author: John Stewart
/// </summary>

namespace Backend.Base.Session
{
    public class SessionService: BaseService, SessionServiceI
    {
        private readonly IMemoryCache _memoryCache;

        public SessionService(IServiceProvider serviceProvider,
            IMemoryCache memoryCache)
            : base(serviceProvider)
        {
            _memoryCache = memoryCache;
        }

        public async Task<SessionEnt> CreateSession(UserEnt user, OrgEnt org, UserConfig userConfig, int sourceApp)
        {
            var key = user.Userid + "-" + Guid.NewGuid().ToString();
            var ses = new SessionEnt
            {
                Key = key,
                User = user,
                Org = org,
                UserConfig = userConfig,
                SourceApp = sourceApp,
            };

            _memoryCache.Set(Key(key), ses);
            _log.Information("CreateSession, key=" + key + ", LoginId=" + user.UserAccountId + ", org id=" + org.Id);
            return ses;
        }

        public async Task RemoveSession(string key)
        {
            var ses = GetSession(key);
            if (ses == null)
            {
                _log.Error("RemoveSession, no session, key=" + key);
                return;
            }

            _log.Information("RemoveSession, key=" + key + ", LoginId=" + ses.User.UserAccountId + ", org id=" + ses.Org.Id);
            _memoryCache.Remove(Key(key));
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
            return GC.CacheKeySessionPrefix + key;
        }

    }
}
