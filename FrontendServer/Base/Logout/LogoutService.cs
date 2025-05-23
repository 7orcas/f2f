using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;


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
            var client = _httpClientFactory.CreateClient("AuthorizedClient");

            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await client.PostAsync("api/Logout/logout", null);
        }

    }
}
