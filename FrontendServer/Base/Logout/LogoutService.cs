using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using GC = FrontendServer.GlobalConstants;

namespace FrontendServer.Base.Logout
{

    public class LogoutService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public LogoutService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }


        public async Task LogoutAsync(string token)
        {
            var client = _httpClientFactory.CreateClient(GC.AuthorizedClientKey);

            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue(GC.BearerKey, token);

            var response = await client.PostAsync(GC.URL_logout, null);
        }

    }
}
