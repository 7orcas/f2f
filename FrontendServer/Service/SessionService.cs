using Microsoft.AspNetCore.Http;

namespace FrontendServer.Service
{

    public class SessionService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SessionService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void SetSessionValue(string key, string value)
        {
            _httpContextAccessor.HttpContext?.Session.SetString(key, value); // Store a string value
        }

        public string? GetSessionValue(string key)
        {
            return _httpContextAccessor.HttpContext?.Session.GetString(key); // Retrieve the string value
        }

        public void ClearSessionValue(string key)
        {
            _httpContextAccessor.HttpContext?.Session.Remove(key); // Remove a specific session value
        }
    }
}
