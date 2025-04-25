using Azure.Core;
using GC = Backend.GlobalConstants;
using Backend.Base.Permission.Ent;
using Backend.Base.Entity;
using System.Runtime.CompilerServices;
using Microsoft.Data.SqlClient;
using System.Security.Cryptography;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.DataProtection.KeyManagement;

namespace Backend.Base.Permission
{
    /// <summary>
    /// Loading permissions on startup
    /// </summary>
    /// <author>John Stewart</author>
    /// <created>March 3, 2025</created>
    /// <license>**Licence**</license>
    public class PermissionInitialiseService: BaseService, PermissionInitialiseServiceI
    {
        private readonly IMemoryCache _memoryCache;
        private const string KEY = "PermissionService_list";

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
                    l.PermissionId = GetId(r, "id");
                    l.Code = GetString(r, "code");
                    l.Description = GetString(r, "descr");
                    list.Add(l);
                }
            );

            _memoryCache.Set(KEY, list);
        }

        /// <summary>
        /// Get all permissions 
        /// </summary>
        /// <returns></returns>
        public List<PermissionEnt> GetPermissions()
        {
            if (_memoryCache.TryGetValue(KEY, out var cachedValue))
            {
                return cachedValue as List<PermissionEnt>;
            }
            return new List<PermissionEnt>();
        }


    }
}
