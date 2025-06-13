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
        
        public PermissionService(IServiceProvider serviceProvider,
            IMemoryCache memoryCache) 
            : base(serviceProvider)
        { 
            _memoryCache = memoryCache;
        }


        public PermissionEnt GetPermissionEnt (string perm)
        {
            if (_memoryCache.TryGetValue(GC.CacheKeyPermList, out var cachedValue))
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
        /// <param name="session"></param>
        /// <returns></returns>
        public async Task<List<PermissionCrudEnt>> LoadEffectivePermissions(SessionEnt session)
        {
            return await LoadEffectivePermissionsInt(session.User.LoginId, session.Org.Id);
        }


        /// <summary>
        /// Load and setup a user's permission/crud settings
        /// The specifig org's permissions are used if exist, then org 0 permission's are the default
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="orgId"></param>
        /// <returns></returns>
        public async Task<List<PermissionCrudEnt>> LoadEffectivePermissionsInt(int userId, int orgId)
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
                    r =>
                    {
                        AddPermission(GetId(r, "permissionId"),
                            GetStringNull(r, "crud"),
                            perms);
                    },
                    new SqlParameter("@userId", userId),
                    new SqlParameter("@orgId", orgId)
                );

                await Sql.Run(sql + "AND r.orgId = " + GC.BaseOrgId,
                    r => {
                        AddPermission(GetId(r, "permissionId"),
                            GetStringNull(r, "crud"),
                            perms);
                    },
                    new SqlParameter("@userId", userId)
                );
                
                return perms.Values.ToList<PermissionCrudEnt>();
            }
            catch
            {
                //ToDo Logme
                return new List<PermissionCrudEnt>();
            }
        }

        private void AddPermission (int permissionId, string crud, Dictionary<int, PermissionCrudEnt> perms)
        {
            if (!perms.ContainsKey(permissionId))
                perms[permissionId] = new PermissionCrudEnt
                {
                    PermissionId = permissionId,
                    Crud = crud
                };
            else
            {
                perms[permissionId].AddCrud(crud);
            }
        }


        /// <summary>
        /// Return a user's permission/crud settings
        /// The specifig org's permissions are used if exist, then org 0 permission's are the default
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="orgId"></param>
        /// <returns></returns>
        public async Task<List<RolePermissionCrudEnt>> GetPermissions(SessionEnt session)
        {
            var list = new List<RolePermissionCrudEnt>();
            try
            {
                var sql = "SELECT r.code as role, p.code as pCode, p.descr as pDescr, rp.crud, r.orgId " +
                    "FROM base.rolePermission rp " +
                        "INNER JOIN base.permission p ON p.id = rp.permissionId " +
                        "INNER JOIN base.zzzRole ur ON ur.roleId = rp.roleId " +
                        "INNER JOIN base.role r ON r.Id = rp.roleId " +
                    "WHERE ur.zzzId = @userId ";
                var by = " ORDER BY r.code, p.code ";

                await Sql.Run(sql + "AND r.orgId = @orgId" + by,
                    r => {
                        list.Add(new RolePermissionCrudEnt() { 
                            Role = GetStringNull(r, "role"),
                            PermissionCode = GetStringNull(r, "pCode"),
                            PermissionsDescr = GetStringNull(r, "pDescr"),
                            Crud = GetStringNull(r, "crud"),
                            OrgId = GetId(r, "orgId")
                        });
                    },
                    new SqlParameter("@userId", session.User.LoginId),
                    new SqlParameter("@orgId", session.Org.Id)
                );

                await Sql.Run(sql + "AND r.orgId = " + GC.BaseOrgId + by,
                    r => {
                        list.Add(new RolePermissionCrudEnt()
                        {
                            Role = GetStringNull(r, "role"),
                            PermissionCode = GetStringNull(r, "pCode"),
                            PermissionsDescr = GetStringNull(r, "pDescr"),
                            Crud = GetStringNull(r, "crud"),
                            OrgId = GetId(r, "orgId")
                        });
                    },
                    new SqlParameter("@userId", session.User.LoginId)
                );

                return list.OrderBy(r => r.Role).ThenBy(r => r.PermissionCode).ToList();
            }
            catch
            {
                //ToDo Logme
                return new List<RolePermissionCrudEnt>();
            }
        }


        public bool IsAuthorizedToAccessEndPoint(SessionEnt session, PermissionAtt perm, CrudAtt crud)
        {
            if (perm == null) return true;
            if (session == null) return false;

            if (crud != null && crud.Action == GC.CrudIgnore) return true;

            var ent = GetPermissionEnt(perm.Name);
            if (ent == null) return false;

            var userCrud = session.GetUserPermissionCrud(ent.PermissionId);
            if (userCrud == null) return false; //permission not found in user profile
            if (crud == null) return true;

            return userCrud.IndexOf(crud.Action) != -1;
        }
    }
}
