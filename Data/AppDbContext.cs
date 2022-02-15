using Microsoft.EntityFrameworkCore;
using ContasService.Models;

namespace ContasService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
        {
            
        }

        public DbSet<Conta> Contas { get; set; }
    }
}