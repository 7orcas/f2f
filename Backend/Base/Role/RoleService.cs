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
        /// <param name="sesson"></param>
        /// <returns></returns>
        public async Task<List<UserAccountRoleEnt>> GetUserRoles(SessionEnt session)
        {
            var list = new List<UserAccountRoleEnt>();
            try
            {
                var sql = "SELECT r.id, r.code, r.descr, r.orgNr, zr.updated, zr.isActive " +
                    "FROM base.userAccRole zr " +
                        "INNER JOIN base.role r ON r.Id = zr.roleId " +
                    "WHERE zr.userAccId = @userId " +
                    "AND r.isActive = 1 ";
                var by = " ORDER BY r.code ";

                await Sql.Run(sql + "AND r.orgNr = @orgNr" + by,
                    r => {
                        list.Add(new UserAccountRoleEnt() {
                            RoleId = GetId(r),
                            Code = GetCode(r),
                            Description = GetDescription(r),
                            orgNr = GetOrgNr(r),
                            Updated = GetUpdated(r),
                            IsActive = IsActive(r),
                        });
                    },
                    new SqlParameter("@userId", session.UserAccount.Id),
                    new SqlParameter("@orgNr", session.Org.Nr)
                );

                await Sql.Run(sql + "AND r.orgNr = " + GC.BaseorgNr + by,
                    r => {
                        list.Add(new UserAccountRoleEnt()
                        {
                            RoleId = GetId(r),
                            Code = GetCode(r),
                            Description = GetDescription(r),
                            orgNr = GetOrgNr(r),
                            Updated = GetUpdated(r),
                            IsActive = IsActive(r),
                        });
                    },
                    new SqlParameter("@userId", session.UserAccount.Id)
                );

                return list.OrderBy(r => r.Code).ToList();
            }
            catch
            {
                //ToDo Logme
                return new List<UserAccountRoleEnt>();
            }
        }


    }
}
