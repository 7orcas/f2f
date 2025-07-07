using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Newtonsoft.Json;
using GC = FrontendServer.GlobalConstants;

/// <summary>
/// Permission and CRUD settings
/// Created: July 2025
/// [*Licence*]
/// Author: John Stewart
/// </summary>

namespace FrontendServer.Base.Permission
{
    public class PermissionService : BaseService
    {
        public List<RolePermissionDto> AllPermissions {  get; private set; }
        public List<PermissionDto> Permissions { get; private set; }


        public PermissionService(ProtectedSessionStorage session,
            ConfigService configService,
            IHttpClientFactory httpClientFactory)
        {
            _session = session;
            _configService = configService;
            _httpClientFactory = httpClientFactory;
        }

        public async Task Initialise()
        {
            try
            {
                if (!_configService.IsInitialized)
                    await _configService.Initialise();

                var config = _configService.Config;
                var client = await GetClient();

                var response = await client.GetAsync(GC.URL_perm_list);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var responseDto = JsonConvert.DeserializeObject<_ResponseDto>(result);

                    if (responseDto.Valid)
                        AllPermissions = JsonConvert.DeserializeObject<List<RolePermissionDto>>(responseDto.Result.ToString());
                }
            }
            catch
            {
            }

            try
            {
                if (!_configService.IsInitialized)
                    await _configService.Initialise();

                var config = _configService.Config;
                var client = await GetClient();

                var response1 = await client.GetAsync(GC.URL_perm_eff);
                if (response1.IsSuccessStatusCode)
                {
                    var result = await response1.Content.ReadAsStringAsync();
                    var responseDto = JsonConvert.DeserializeObject<_ResponseDto>(result);

                    if (responseDto.Valid)
                        Permissions = JsonConvert.DeserializeObject<List<PermissionDto>>(responseDto.Result.ToString());
                }
            }
            catch
            {
            }
        }

    }
}
