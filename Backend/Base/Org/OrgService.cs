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

        public async Task<OrgEnt> GetOrg(int nr)
        {
            var org = _memoryCache.Get<OrgEnt>(GC.CacheKeyOrgPrefix + nr);
            if (org != null) return org;

            org = new OrgEnt();
            try
            {
                await Sql.Run(
                    "SELECT * FROM base.org "
                    + "WHERE nr = @nr ",
                    r => {
                        org.Id = GetId(r);
                        org.Nr = GetNr(r);
                        org.Code = GetCode(r);
                        org.Description = GetDescription(r);
                        org.Updated = GetUpdated(r);
                        org.IsActive = IsActive(r);
                        org.LangLabelVariant = GetIntNull(r, "langLabelVariant");
                    },
                    new SqlParameter("@nr", nr)
                );
                _memoryCache.Set(GC.CacheKeyOrgPrefix + org.Nr, org);
                return org;
            }
            catch 
            {
                return null;
            }
        }

       
    }
}
