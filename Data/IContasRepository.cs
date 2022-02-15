using ContasService.Models;

namespace ContasService.Data
{
    public interface IContasRepository
    {
        void CreateConta(Conta conta);
        void DeleteContas(IEnumerable<Conta> contas);
        IEnumerable<Conta> GetContasByCpfPortador(string portadorCpf);
        Conta? GetContaByNumero(string numero);
        IEnumerable<Conta> GetContas();

        void SaveChanges();
    }
}