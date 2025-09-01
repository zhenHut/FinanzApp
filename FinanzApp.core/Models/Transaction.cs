using System.ComponentModel.DataAnnotations.Schema;

namespace FinanzApp.core.Models
{
    [Table("Transaction")]
    public class Transaction
    {
        public int Id { get; set; }
        public decimal? BudgetAmount { get; set; }
        public decimal Amount { get; set; }

        public string Name {  get; set; } = string.Empty;

        public string? Description { get; set; } = string.Empty;

        public TransactionType TransactionType { get; set; }

        public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.Now);

        public string? Category { get; set; } = string.Empty;

    }
}
