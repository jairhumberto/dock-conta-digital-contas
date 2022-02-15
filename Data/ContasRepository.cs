using ContasService.Models;

namespace ContasService.Data
{
    public class ContasRepository : IContasRepository
    {
        private readonly AppDbContext _context;

        public ContasRepository(AppDbContext context)
        {
            _context = context;
        }

        public void CreateConta(Conta conta)
        {
            if (conta == null)
            {
                throw new ArgumentNullException(nameof(conta));
            }

            _context.Add(conta);
        }

        public void DeleteConta(Conta conta)
        {
            if (conta == null)
            {
                throw new ArgumentNullException(nameof(conta));
            }

            _context.Remove(conta);
        }

        public Conta? GetContaByCpfPortador(string portadorCpf)
        {
            return _context.Contas.FirstOrDefault(e => portadorCpf == e.PortadorCpf);
        }

        public Conta? GetContaByNumero(string numero)
        {
            return _context.Contas.FirstOrDefault(e => numero == e.Numero);
        }

        public IEnumerable<Conta> GetContas()
        {
            return _context.Contas.ToList();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}