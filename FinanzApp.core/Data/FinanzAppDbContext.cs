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
                optionsBuilder.UseSqlite($"Data Source={path}");
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
