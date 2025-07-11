using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using System.Text;


/// <summary>
/// Configuration methods for Http stuff
/// Created: June 2025
/// [*Licence*]
/// Author: John Stewart
/// </summary>

namespace FrontendServer.Base._Base
{
    public partial class BasePage
    {
        //Http Client
        public async Task<HttpClient> GetClient() => await HS.GetClient();

        public async Task<T> GetAsync<T>(string url)
        {
            return await GetAsync<T>(url, false);
        }

        public async Task<T> GetAsync<T>(string url, bool surpressLoading)
        {
            _isLoading = !surpressLoading;

            var client = await GetClient();

            var response = await client.GetAsync(url);

            try
            {
                response.EnsureSuccessStatusCode();
                var r = await response.Content.ReadAsStringAsync();
                var dto = JsonConvert.DeserializeObject<_ResponseDto>(r);
                _statusCode = dto.StatusCode;
                _isLoading = false;

                if (dto.Valid)
                    return JsonConvert.DeserializeObject<T>(dto.Result.ToString());

                _errorMessage = new MarkupString(dto.ErrorMessage);
            }
            catch
            {
                _statusCode = -1;
                try
                {
                    var r = await response.Content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(r))
                        _errorMessage = new MarkupString(r);
                    //ToDo label
                    else
                        _errorMessage = new MarkupString("Opps, something went wrong");
                }
                catch (Exception ex)
                {
                    _errorMessage = new MarkupString($"Exception occurred: {ex.Message}");
                }
            }

            _isLoading = false;
            _isError = true;
            return default;
        }

        protected async Task<HttpResponseMessage> PostAsync<T>(string url, _BaseDto<T> dto) where T : _BaseDto<T>
        {
            _isSaving = true;
            var client = await GetClient();

            var json = System.Text.Json.JsonSerializer.Serialize((T)dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, content);

            _isSaving = false;
            return response;
        }
    }
}
