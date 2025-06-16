using Backend.Base.Entity;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Caching.Memory;
using GC = Backend.GlobalConstants;

/// <summary>
/// Organisation methods
/// Created: March 2025
/// [*Licence*]
/// Author: John Stewart
/// </summary>


namespace Backend.Base.Org
{
    public class OrgService: BaseService, OrgServiceI
    {
        private readonly IMemoryCache _memoryCache;
        
        public OrgService(IServiceProvider serviceProvider,
            IMemoryCache memoryCache) 
            : base(serviceProvider) 
        {
            _memoryCache = memoryCache;
        }

        public async Task<List<OrgEnt>> GetOrgs()
        {
            var list = new List<OrgEnt>();
            await Sql.Run(
                    "SELECT * FROM base.org ",
                    r => {
                        var org = OrgLoad.Load(r);
                        list.Add(org);
                        _memoryCache.Set(GC.CacheKeyOrgPrefix + org.Nr, org);
                    }
                );
            return list;
        }

        public async Task<OrgEnt> GetOrg(int nr)
        {
            var org = _memoryCache.Get<OrgEnt>(GC.CacheKeyOrgPrefix + nr);
            if (org != null) return org;

            try
            {
                await Sql.Run(
                    "SELECT * FROM base.org "
                    + "WHERE nr = @nr ",
                    r => {
                        org = OrgLoad.Load(r);
                        _memoryCache.Set(GC.CacheKeyOrgPrefix + org.Nr, org);
                    },
                    new SqlParameter("@nr", nr)
                );
                                
                return org;
            }
            catch 
            {
                return null;
            }
        }

    }
}
