using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FinanzApp.core.Infrastructure
{
    public class FinanzAppDbContextFactory : IDesignTimeDbContextFactory<FinanzAppDbContext>
    {
        public FinanzAppDbContext CreateDbContext(string[] args)
        {
            var options = new DbContextOptionsBuilder<FinanzAppDbContext>()
                .UseSqlite("Data Source=finanzapp.db") // deinen Pfad/Dateinamen einsetzen
                .Options;

            return new FinanzAppDbContext(options);
        }
    }
}
