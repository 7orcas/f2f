using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace FrontendServer.Service
{
    public class LocalStorageServiceDELETE_ME
    {
        private readonly IJSRuntime _jsRuntime;

        public LocalStorageServiceDELETE_ME(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task SetItem(string key, string value)
        {
            await _jsRuntime.InvokeVoidAsync("localStorageHelper.setItem", key, value);
        }

        public async Task<string> GetItem(string key)
        {
            return await _jsRuntime.InvokeAsync<string>("localStorageHelper.getItem", key);
        }

        public async Task RemoveItem(string key)
        {
            await _jsRuntime.InvokeVoidAsync("localStorageHelper.removeItem", key);
        }
    }
}
