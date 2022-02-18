using ContasService.Models;

namespace ContasService.Data
{
    public interface IPortadoresRepository
    {
        void CreatePortador(Portador portador);
        void DeletePortador(Portador portador);
        Portador? GetPortadorByCpf(string cpf);

        void SaveChanges();
    }
}