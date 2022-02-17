using ContasService.Dtos;

namespace ContasService.SyncDataServices.Http
{
    public interface IOperacaoDataClient
    {
        Task SendContaToOperacao(ContaReadDto conta); 
    }
}