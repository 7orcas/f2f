using Azure.Core;
using GC = Backend.GlobalConstants;
using Backend.Base.Role.Ent;
using Backend.Base.Entity;
using System.Runtime.CompilerServices;
using Microsoft.Data.SqlClient;
using System.Security.Cryptography;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.DataProtection.KeyManagement;

namespace Backend.Base.Role
{
    /// <summary>
    /// Manage loading and processing roles
    /// </summary>
    /// <author>John Stewart</author>
    /// <created>April 22, 2025</created>
    /// <license>**Licence**</license>
    public class RoleService: BaseService, RoleServiceI
    {

        public RoleService(IServiceProvider serviceProvider) : base(serviceProvider) { }

        /// <summary>
        /// Return a user's Roles
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="orgId"></param>
        /// <returns></returns>
        public async Task<List<UserRoleEnt>> GetUserRoles(SessionEnt session)
        {
            _auditService.ReadList(session, GC.EntityRole, null);

            var list = new List<UserRoleEnt>();
            try
            {
                var sql = "SELECT r.id, r.code, r.descr, r.orgId, zr.updated, zr.isActive " +
                    "FROM base.zzzRole zr " +
                        "INNER JOIN base.role r ON r.Id = zr.roleId " +
                    "WHERE zr.zzzId = @userId " +
                    "AND r.isActive = 1 ";
                var by = " ORDER BY r.code ";

                await Sql.Run(sql + "AND r.orgId = @orgId" + by,
                    r => {
                        list.Add(new UserRoleEnt() {
                            RoleId = GetId(r, "id"),
                            Code = GetString(r, "code"),
                            Description = GetString(r, "descr"),
                            OrgId = GetId(r, "orgId"),
                            Updated = GetDateTime(r, "updated"),
                            IsActive = GetBoolean(r, "isActive"),
                        });
                    },
                    new SqlParameter("@userId", session.User.LoginId),
                    new SqlParameter("@orgId", session.Org.Id)
                );

                await Sql.Run(sql + "AND r.orgId = " + GC.BaseOrgId + by,
                    r => {
                        list.Add(new UserRoleEnt()
                        {
                            RoleId = GetId(r, "id"),
                            Code = GetString(r, "code"),
                            Description = GetString(r, "descr"),
                            OrgId = GetId(r, "orgId"),
                            Updated = GetDateTime(r, "updated"),
                            IsActive = GetBoolean(r, "isActive"),
                        });
                    },
                    new SqlParameter("@userId", session.User.LoginId)
                );

                return list.OrderBy(r => r.Code).ToList();
            }
            catch
            {
                //ToDo Logme
                return new List<UserRoleEnt>();
            }
        }


    }
}
