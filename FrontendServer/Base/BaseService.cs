using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;


namespace FrontendServer.Base
{
    public class BaseService
    {
        private readonly ProtectedSessionStorage _protectedSessionStore;

        public BaseService(ProtectedSessionStorage protectedSessionStore)
        {
            _protectedSessionStore = protectedSessionStore;
        }

    }
}
