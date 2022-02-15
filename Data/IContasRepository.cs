using ContasService.Models;

namespace ContasService.Data
{
    public interface IContasRepository
    {
        void CreateConta(Conta conta);
        void DeleteConta(Conta conta);
        Conta? GetContaByCpfPortador(string portadorCpf);
        Conta? GetContaByNumero(string numero);
        IEnumerable<Conta> GetContas();

        void SaveChanges();
    }
}