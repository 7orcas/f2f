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
    /// Manage loading and processing permissions for logged user
    /// Tables and relationships:
    ///  - base.role: roles, contains BaseOrgId general roles and OrgId specific roles
    ///  - base.permission: hardcoded permissions in controllers (not OrgId related)
    ///  - base.rolePermission: relation between role and permission with allowed crud value
    ///  - base.zzz: user login with allowed OrgIds
    ///  - base.zzzRole: relation bewteen zzz (user) and role (BaseOrgId and OrgId user is logging into)
    /// </summary>
    /// <author>John Stewart</author>
    /// <created>March 3, 2025</created>
    /// <license>**Licence**</license>
    public class PermissionService: BaseService, PermissionServiceI
    {
        private readonly IMemoryCache _memoryCache;
        private const string KEY = "PermissionService_list";

        public PermissionService() { }

        public PermissionService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }


        public PermissionEnt GetPermissionEnt (string perm)
        {
            if (_memoryCache.TryGetValue(KEY, out var cachedValue))
            {
                var list = cachedValue as List<PermissionEnt>;
                return list.Find(p => p.Code.Equals(perm));
            }
            return null;
        }

        /// <summary>
        /// Load and setup a user's permission/crud settings
        /// The specifig org's permissions are used if exist, then org 0 permission's are the default
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="orgId"></param>
        /// <returns></returns>
        public async Task<List<PermissionCrudEnt>> LoadPermissions(int userId, int orgId)
        {
            var perms = new Dictionary<int, PermissionCrudEnt>();
            try
            {
                var sql = "SELECT rp.permissionId, rp.crud " +
                    "FROM base.rolePermission rp " +
                        "INNER JOIN base.zzzRole ur ON ur.roleId = rp.roleId " +
                        "INNER JOIN base.role r ON r.Id = rp.roleId " +
                    "WHERE ur.zzzId = @userId ";

                await Sql.Run(sql + "AND r.orgId = @orgId",
                    r => {
                        var l = new PermissionCrudEnt();
                        l.PermissionId = GetId(r, "permissionId");
                        l.Crud = GetString(r, "crud");
                        perms[l.PermissionId] = l;
                    },
                    new SqlParameter("@userId", userId),
                    new SqlParameter("@orgId", orgId)
                );

                await Sql.Run(sql + "AND r.orgId = " + GC.BaseOrgId,
                    r => {
                        var l = new PermissionCrudEnt();
                        l.PermissionId = GetId(r, "permissionId");
                        l.Crud = GetString(r, "crud");
                        if (!perms.ContainsKey(l.PermissionId))
                            perms[l.PermissionId] = l;
                    },
                    new SqlParameter("@userId", userId)
                );
                
                var list = perms.Values.ToList<PermissionCrudEnt>();
                foreach(var p in list)
                {
                    p.Crud = p.Crud.ToLower();
                    if (p.Crud.Contains(GC.CrudCreate) &&
                        p.Crud.Contains(GC.CrudRead) &&
                        p.Crud.Contains(GC.CrudUpdate) &&
                        p.Crud.Contains(GC.CrudDelete))
                        p.Crud = GC.CrudAll;
                }

                return perms.Values.ToList<PermissionCrudEnt>();
            }
            catch
            {
                //ToDo Logme
                return new List<PermissionCrudEnt>();
            }
        }


        public bool IsAuthorizedToAccessEndPoint(SessionEnt session, PermissionAtt perm, CrudAtt crud)
        {
            if (perm == null) return true;
            if (session == null) return false;

            var ent = GetPermissionEnt(perm.Name);
            if (ent == null) return false;

            var userCrud = session.GetUserPermissionCrud(ent.PermissionId);
            if (userCrud == null) return false; //permission not found in user profile
            if (crud == null || userCrud.Equals(GC.CrudAll)) return true;

            return userCrud.IndexOf(crud.Action) != -1;
        }


        public string test()
        {
            return "TEST OK";
        }
    }
}
