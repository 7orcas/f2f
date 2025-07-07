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
    ///  - hardcoded permissions in controllers (not OrgId related)
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
                return list.Find(p => p.Permission.Equals(perm));
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
            return await LoadEffectivePermissionsInt(session.UserAccount.Id, session.Org.Id);
        }


        /// <summary>
        /// Load and setup a user's permission/crud settings
        /// The specific org's permissions are used if exist, then org 0 permission's are the default
        /// </summary>
        /// <param name="userAccountId"></param>
        /// <param name="orgId"></param>
        /// <returns></returns>
        public async Task<List<PermissionCrudEnt>> LoadEffectivePermissionsInt(long userAccountId, long orgId)
        {
            var perms = new Dictionary<string, PermissionCrudEnt>(); //permision / crud
            try
            {
                var sql = "SELECT rp.permission, rp.crud " +
                    "FROM base.rolePermission rp " +
                        "INNER JOIN base.userAccRole ur ON ur.roleId = rp.roleId " +
                        "INNER JOIN base.role r ON r.Id = rp.roleId " +
                    "WHERE ur.userAccId = @userAccId ";

                await Sql.Run(sql + "AND r.orgId = @orgId",
                    r =>
                    {
                        AddPermission(
                            GetString(r, "permission"),
                            GetString(r, "crud"),
                            perms);
                    },
                    new SqlParameter("@userAccId", userAccountId),
                    new SqlParameter("@orgId", orgId)
                );

                await Sql.Run(sql + "AND r.orgId = " + GC.BaseOrgId,
                    r => {
                        AddPermission(
                            GetString(r, "permission"),
                            GetString(r, "crud"),
                            perms);
                    },
                    new SqlParameter("@userAccId", userAccountId)
                );
                
                return perms.Values.ToList<PermissionCrudEnt>();
            }
            catch
            {
                //ToDo Logme
                return new List<PermissionCrudEnt>();
            }
        }

        private void AddPermission (string permission, string crud, Dictionary<string, PermissionCrudEnt> perms)
        {
            if (!perms.ContainsKey(permission))
                perms[permission] = new PermissionCrudEnt
                {
                    Permission = permission,
                    Crud = crud
                };
            else
            {
                perms[permission].AddCrud(crud);
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
                var sql = "SELECT r.code as role, rp.permission, rp.crud, r.orgId " +
                    "FROM base.rolePermission rp " +
                        "INNER JOIN base.userAccRole uar ON uar.roleId = rp.roleId " +
                        "INNER JOIN base.role r ON r.Id = rp.roleId " +
                    "WHERE uar.userAccId = @userAccId ";
                var by = " ORDER BY r.code, rp.permission ";

                await Sql.Run(sql + "AND r.orgId = @orgId" + by,
                    r => {
                        list.Add(new RolePermissionCrudEnt() { 
                            Role = GetStringNull(r, "role"),
                            Permission = GetString(r, "permission"),
                            Crud = GetString(r, "crud"),
                            OrgId = GetOrgId(r)
                        });
                    },
                    new SqlParameter("@userAccId", session.UserAccount.Id),
                    new SqlParameter("@orgId", session.Org.Id)
                );

                await Sql.Run(sql + "AND r.orgId = " + GC.BaseOrgId + by,
                    r => {
                        list.Add(new RolePermissionCrudEnt()
                        {
                            Role = GetStringNull(r, "role"),
                            Permission = GetString(r, "permission"),
                            Crud = GetString(r, "crud"),
                            OrgId = GetOrgId(r)
                        });
                    },
                    new SqlParameter("@userAccId", session.UserAccount.Id)
                );

                return list.OrderBy(r => r.Role).ThenBy(r => r.Permission).ToList();
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
            if (session.UserAccount.IsService()) return true;

            if (crud != null && crud.Action == GC.CrudIgnore) return true;

            var ent = GetPermissionEnt(perm.Name);
            if (ent == null) return false;

            var userCrud = session.GetUserPermissionCrud(ent.Permission);
            if (userCrud == null) return false; //permission not found in user profile
            if (crud == null) return true;

            return userCrud.IndexOf(crud.Action) != -1;
        }
    }
}
