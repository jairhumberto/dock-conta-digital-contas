using ContasService.Dtos;

namespace ContasService.SyncDataServices.Http
{
    public interface IOperacoesServiceClient
    {
        Task CreateConta(ContaReadDto contaReadDto); 
    }
}