using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.JSInterop;

namespace FrontendServer.Service
{
    public class AuthorizationMessageHandler : DelegatingHandler
    {
        // private readonly SessionTokenStorageService _tokenStorage;
        private readonly ProtectedSessionStorage _protectedSessionStorage;
        private readonly IJSRuntime _jsRuntime;

        //private readonly SessionService _session;

        public AuthorizationMessageHandler(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }


        //public AuthorizationMessageHandler(//SessionTokenStorageService tokenStorage,
        //    ProtectedSessionStorage protectedSessionStorage) //, SessionService session)
        //{
        //    // _tokenStorage = tokenStorage;
        //    _protectedSessionStorage = protectedSessionStorage;
        //    //_session = session;
        //}

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            //var token = await _tokenStorage.GetTokenAsync();
            //var token = _session.GetSessionValue("token");
            string token = null;
            //var result = await _protectedSessionStorage.GetAsync<string>("token");
            //if (result.Success) 
            //    token = result.Value;

            //var token = await _jsRuntime.InvokeAsync<string>("sessionStorage.getItem", "token");

            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
