using Microsoft.JSInterop;

namespace FrontendServer.Service
{
    public class SessionTokenStorageService
    {
        private readonly IJSRuntime _jsRuntime;
        private bool _isInitialized = false;

        public SessionTokenStorageService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        private async Task EnsureInitializedAsync()
        {
            if (!_isInitialized)
            {
                // Automatically initialize before performing interop
                _isInitialized = true;
            }
        }

        public async Task InitializeAsync()
        {
            _isInitialized = true;
        }

        public async Task SetTokenAsync(string token)
        {
            await EnsureInitializedAsync(); // Ensure initialization
            await _jsRuntime.InvokeVoidAsync("sessionStorage.setItem", "token", token); // Save token to session storage
        }

        public async Task<string?> GetTokenAsync()
        {
            await EnsureInitializedAsync(); // Ensure initialization
            return await _jsRuntime.InvokeAsync<string>("sessionStorage.getItem", "token"); // Retrieve token from session storage
        }

        public async Task ClearTokenAsync()
        {
            await EnsureInitializedAsync(); // Ensure initialization
            await _jsRuntime.InvokeVoidAsync("sessionStorage.removeItem", "token"); // Remove token from session storage
        }
    }
}
