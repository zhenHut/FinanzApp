
using FinanzApp.core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

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
        public DbSet<Transaction> Transactions => Set<Transaction>();
        public DbSet<Category> Categories => Set<Category>();

        #endregion

        #region Methods

        protected override void OnModelCreating(ModelBuilder m)
        {
            // DateOnly-Konverter für SQLite
            var dateOnlyConverter = new ValueConverter<DateOnly, string>(
                v => v.ToString("yyyy-MM-dd"),
                v => DateOnly.Parse(v));

            m.Entity<Transaction>()
             .Property(t => t.Date)
             .HasConversion(dateOnlyConverter);

            // FK-Relation (nullable, SetNull beim Löschen)
            m.Entity<Transaction>()
             .HasOne(t => t.Category)
             .WithMany()
             .HasForeignKey(t => t.CategoryId)
             .OnDelete(DeleteBehavior.SetNull);
        }
    }
    #endregion



}

