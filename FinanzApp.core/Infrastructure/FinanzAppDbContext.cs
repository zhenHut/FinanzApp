
using FinanzApp.core.Models;
using Microsoft.EntityFrameworkCore;

namespace FinanzApp.core.Infrastructure
{
    public class FinanzAppDbContext : DbContext
    {

        #region Constructor
        public FinanzAppDbContext(DbContextOptions<FinanzAppDbContext> options) : base(options)
        {

        }

        #endregion

        #region Properties
        public DbSet<Transaction> Transactions { get; set; }

        #endregion



    }
}
