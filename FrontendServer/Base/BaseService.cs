using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using GC = FrontendServer.GlobalConstants;

namespace FrontendServer.Base
{
    public class BaseService
    {
        protected CacheService _Cache;
        protected Dictionary<string, LangLabelDto> _labels;

        public BaseService(CacheService Cache)
        {
            _Cache = Cache;
            SetLabels();
        }

        public async Task<(T obj, MarkupString message)> Call<T>(HttpClient client, string url)
        {
            T rtn = default;

            try
            {
                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var responseDto = JsonConvert.DeserializeObject<_ResponseDto>(result);
                    var message = null as string;

                    if (responseDto.Valid)
                        rtn = JsonConvert.DeserializeObject<T>(responseDto.Result.ToString());
                    else
                        message = responseDto.ErrorMessage;

                    return (rtn, new MarkupString(message));
                }
                throw new Exception("Unknown Error");
            }
            catch (Exception ex)
            {
                return (rtn, new MarkupString("<h3>" + GetLabel("apiError") + "</h3>Error:" + ex.Message));
            }
        }


        public void SetLabels()
        {
           _labels = _Cache.GetLabels("en"); //ToDo
        }

        public string GetLabel(string labelCode)
        {
            return GetLabel(labelCode, labelCode);
        }

        public string GetLabel(string labelCode, string fallBack)
        {
            if (_labels.ContainsKey(labelCode))
                return _labels[labelCode].Label;
            return "?" + fallBack;
        }

    }
}
