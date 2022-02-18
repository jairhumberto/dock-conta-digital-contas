using System.Text;
using System.Text.Json;
using ContasService.Dtos;

namespace ContasService.SyncDataServices.Http
{
    public class HttpOperacoesServiceClient : IOperacoesServiceClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public HttpOperacoesServiceClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task CreateConta(ContaReadDto contaReadDto)
        {
            var httpContent = new StringContent(JsonSerializer.Serialize(contaReadDto), Encoding.UTF8,
                    "application/json");
            var response = await _httpClient.PostAsync($"{_configuration["OperacoesService"]}", httpContent);

            if(!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"--->{await response.Content.ReadAsStringAsync()}");
                //throw new HttpRequestException();
            }
        }
    }
}