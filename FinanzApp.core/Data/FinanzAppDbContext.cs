using FinanzApp.core.Interface;
using FinanzApp.core.Model;
using Microsoft.EntityFrameworkCore;

namespace FinanzApp.core.Data
{
    public class FinanzAppDbContext : DbContext, INotificationRequest
    {
        public DbSet<Transaction> Transactions { get; set; }

        public event EventHandler<string>? NotificationRequested;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            try
            {
                var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "finanzapp.db");
                var connectionString = $"Data Source={path}; Password=Z2n54lsü2!q";
                optionsBuilder.UseSqlite(connectionString);
            }
            catch (Exception ex)
            {
                Notification("Fehler beim Konfigurieren der Datenbank: " + ex.Message);
            }
        }

        private void Notification(string message)
        {
            NotificationRequested?.Invoke(this,message);
        }
    }
}
