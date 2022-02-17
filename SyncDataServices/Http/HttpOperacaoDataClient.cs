using System.Text;
using System.Text.Json;
using ContasService.Dtos;

namespace ContasService.SyncDataServices.Http
{
    public class HttpOperacaoDataClient : IOperacaoDataClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public HttpOperacaoDataClient(HttpClient httpClient, IConfiguration configuration)
        {
            
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task SendContaToOperacao(ContaReadDto conta)
        {
            var httpContent = new StringContent(
                JsonSerializer.Serialize(conta),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync($"{_configuration["OperacoesService"]}", httpContent);

            if(!response.IsSuccessStatusCode)
            {
                throw new HttpSyncException();
            }
        }
    }
}