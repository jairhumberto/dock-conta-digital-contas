using ContasService.Models;

namespace ContasService.Data
{
    public interface IPortadoresRepository
    {
        void CreatePortador(Portador portador);
        void DeletePortador(Portador portador);
        Portador? GetPortadorByCpf(string cpf);
        IEnumerable<Portador> GetPortadores();

        void SaveChanges();
    }
}