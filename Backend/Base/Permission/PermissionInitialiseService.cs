using GC = Backend.GlobalConstants;
using Microsoft.Extensions.Caching.Memory;

/// <summary>
/// Loading permissions on startup
/// Created: March 2025
/// [*Licence*]
/// Author: John Stewart
/// </summary>

namespace Backend.Base.Permission
{
    public class PermissionInitialiseService: BaseService, PermissionInitialiseServiceI
    {
        private readonly IMemoryCache _memoryCache;
        
        public PermissionInitialiseService(IServiceProvider serviceProvider,
            IMemoryCache memoryCache)
            : base(serviceProvider)
        {
            _memoryCache = memoryCache;
        }


        /// <summary>
        /// Load all permissions and store 
        /// </summary>
        /// <returns></returns>
        public async void InitialisePermissions()
        {
            var list = new List<PermissionEnt>();

            var sql = "SELECT * " +
                    "FROM base.permission ";

            await Sql.Run(sql,
                r => {
                    var l = new PermissionEnt();
                    l.Id = GetId(r, "id");
                    l.Code = GetStringNull(r, "code");
                    l.Description = GetStringNull(r, "descr");
                    list.Add(l);
                }
            );

            _memoryCache.Set(GC.CacheKeyPermList, list);
        }

        /// <summary>
        /// Get all permissions 
        /// </summary>
        /// <returns></returns>
        public List<PermissionEnt> GetPermissions()
        {
            if (_memoryCache.TryGetValue(GC.CacheKeyPermList, out var cachedValue))
            {
                return cachedValue as List<PermissionEnt>;
            }
            return new List<PermissionEnt>();
        }


    }
}
