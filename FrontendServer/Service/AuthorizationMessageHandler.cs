namespace FrontendServer.Service
{
    public class AuthorizationMessageHandler : DelegatingHandler
    {
        private readonly TokenStorageService _tokenStorage;

        public AuthorizationMessageHandler(TokenStorageService tokenStorage)
        {
            _tokenStorage = tokenStorage;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(_tokenStorage.JwtToken))
            {
                request.Headers.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _tokenStorage.JwtToken);
            }
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
