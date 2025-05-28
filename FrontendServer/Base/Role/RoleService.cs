using System.Collections.Generic;
using System.Net.Http;
using FrontendServer.Base.Cache;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Newtonsoft.Json;
using GC = FrontendServer.GlobalConstants;

namespace FrontendServer.Base.Role
{
    public class RoleService : BaseService
    {
        public RoleService(CacheService Cache) : base (Cache) { }

        public async Task<(List<UserRoleDto> roles, MarkupString message)> RolesAsync(HttpClient client)
        {
            return await Call<List<UserRoleDto>>(client, GC.URL_role_list);
        }

    }
}
