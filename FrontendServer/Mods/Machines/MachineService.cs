using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;


namespace FrontendServer.Mods.Machines
{

    //May not need this

    public class MachineService 
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public MachineService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }


        public async Task<(List<MachineDto> machines, string message)> MachinesAsync(string token)
        {
            var client = _httpClientFactory.CreateClient("AuthorizedClient");

            //var tokenResponse = await client.GetAsync("api/Token");
            //var token = await tokenResponse.Content.ReadAsStringAsync();

            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            return (new List<MachineDto>(), "XXXX?");

            //var list = new List<MachineDto>();
            //var message = "";

            //var response = await client.GetAsync("api/Machine/list");
            //try
            //{
            //    var result = await response.Content.ReadAsStringAsync();
            //    var responseDto = JsonConvert.DeserializeObject<_ResponseDto>(result);

            //    if (responseDto.Valid)
            //        list = JsonConvert.DeserializeObject<List<MachineDto>>(responseDto.Result.ToString());
            //    else
            //        message = responseDto.ErrorMessage;


            //    return (list, message);
            //}
            //catch 
            //{
            //    var result = await response.Content.ReadAsStringAsync();
            //    if (!string.IsNullOrEmpty(result)) 
            //        return (list, result);
                
            //    //ToDo label
            //    return (list, "Opps, something went wrong?");
            //}
        }
    }
}
