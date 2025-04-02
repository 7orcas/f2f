using System.Net.Http;
using Newtonsoft.Json;


namespace FrontendServer.Modules.Machines
{

    //May not need this

    public class MachineService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public MachineService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }


        public async Task<List<MachineDto>> MachinesAsync()
        {
            var client = _httpClientFactory.CreateClient("BackendApi");

            var response = await client.GetAsync("api/Machine/machines");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var responseDto = JsonConvert.DeserializeObject<_ResponseDto>(result);
                var list = JsonConvert.DeserializeObject<List<MachineDto>>(responseDto.Result.ToString());

Console.WriteLine("Get Machines");

                return list;

                //successMessage = responseDto.SuccessMessage;
                //successMessage += "  Token:" + token.Token;

                //NavigationManager.NavigateTo("https://localhost:7170", true);
                // Save token, e.g., in localStorage if this were Blazor WebAssembly
            }
            else
            {
                //errorMessage = "Invalid username or password + "
                //    + test;

            }
            return new List<MachineDto>();
        }
    }
}
