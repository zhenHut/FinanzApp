using FinanzApp.core.Model;
using Microsoft.EntityFrameworkCore;

namespace FinanzApp.core.Data
{
    public class FinanzAppDbContext : DbContext
    {
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("DataSource=data/finanzapp.db");
        }
    }
}
