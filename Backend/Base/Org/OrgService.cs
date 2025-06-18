using Backend.Base.Entity;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Cryptography;
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

        public async Task<List<OrgEnt>> GetOrgList()
        {
            var list = new List<OrgEnt>();
            await Sql.Run(
                    "SELECT * FROM base.org ",
                    r => {
                        var org = new OrgEnt();
                        org.Id = GetInt(r, "id");
                        org.Code = GetCode(r);
                        org.Description = GetDescription(r);
                        org.Updated = GetUpdated(r);
                        org.IsActive = IsActive(r);
                        list.Add(org);
                    }
                );
            return list;
        }

        public async Task<OrgEnt> GetOrg(int id)
        {
            var org = _memoryCache.Get<OrgEnt>(GC.CacheKeyOrgPrefix + id);
            if (org != null) return org;

            try
            {
                await Sql.Run(
                    "SELECT * FROM base.org "
                    + "WHERE nr = @nr ",
                    r => {
                        org = OrgLoad.Load(r);
                        _memoryCache.Set(GC.CacheKeyOrgPrefix + org.Id, org);
                    },
                    new SqlParameter("@nr", id)
                );
                                
                return org;
            }
            catch 
            {
                return null;
            }
        }

        public async Task UpdateOrg(OrgEnt org)
        {
            org.Encode();
            await Sql.Execute(
                    "UPDATE base.org " +
                    "SET " +
                        Update("code", org.Code) +
                        Update("descr", org.Description) +
                        Update("encoded", org.Encoded) +
                        Update("updated", org.Updated) +
                        Update("isActive", org.IsActive) +
                        Update("langCode", org.LangCode) +
                        NoComma(Update("langLabelVariant", org.LangLabelVariant)) +
                    " WHERE id = " + org.Id
            );
            _memoryCache.Set(GC.CacheKeyOrgPrefix + org.Id, org);
        }

    }
}
