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


            //var list = new List<UserRoleDto>();
            //var message = "";

            //var response = await client.GetAsync(GC.URL_role_list);
            //if (response.IsSuccessStatusCode)
            //{
            //    var result = await response.Content.ReadAsStringAsync();
            //    var responseDto = JsonConvert.DeserializeObject<_ResponseDto>(result);

            //    if (responseDto.Valid)
            //        list = JsonConvert.DeserializeObject<List<UserRoleDto>>(responseDto.Result.ToString());
            //    else
            //        message = responseDto.ErrorMessage;


            //    return (list, message);
            //}

            ////ToDo label
            //return (list, "Opps, something went wrong?");
        }

    }
}
