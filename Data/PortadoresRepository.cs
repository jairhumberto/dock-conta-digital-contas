using ContasService.Models;

namespace ContasService.Data
{
    public class PortadoresRepository : IPortadoresRepository
    {
        private readonly AppDbContext _context;

        public PortadoresRepository(AppDbContext context)
        {
            _context = context;
        }

        public void CreatePortador(Portador portador)
        {
            if (portador == null)
            {
                throw new ArgumentNullException(nameof(portador));
            }

            _context.Add(portador);
        }

        public void DeletePortador(Portador portador)
        {
            if (portador == null)
            {
                throw new ArgumentNullException(nameof(portador));
            }

            _context.Remove(portador);
        }

        public Portador? GetPortadorByCpf(string cpf)
        {
            return _context.Portadores.FirstOrDefault(e => cpf == e.Cpf);
        }

        public IEnumerable<Portador> GetPortadores()
        {
            return _context.Portadores;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}