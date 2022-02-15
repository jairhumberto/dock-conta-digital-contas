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
            _context.ChangeTracker.DetectChanges();

            conta.Numero = conta.Id.ToString();
        }

        public void DeleteContas(IEnumerable<Conta> contas)
        {
            _context.RemoveRange(contas);
        }

        public IEnumerable<Conta> GetContasByCpfPortador(string portadorCpf)
        {
            return _context.Contas.Where(e => portadorCpf == e.PortadorCpf).ToList();
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