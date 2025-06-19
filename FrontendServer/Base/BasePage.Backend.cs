
using System.Text.Json;
using Common.DTO;
using System.Net;


using System.Text;

/// <summary>
/// Backend API utility methods
/// Created: June 2025
/// [*Licence*]
/// Author: John Stewart
/// </summary>

namespace FrontendServer.Base
{
    public partial class BasePage
    {
        protected async Task<HttpResponseMessage> PostAsync<T>(string url, _BaseDto<T> dto) where T : _BaseDto<T>
        {
            _isSaving = true;
            var client = await SetClient();

            var json = JsonSerializer.Serialize((T)dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, content);

            _isSaving = false;
            return response;
        }
    }
}
