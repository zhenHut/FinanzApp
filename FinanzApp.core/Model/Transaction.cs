using FinanzApp.core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace FinanzApp.core.Model
{
    public class Transaction
    {
        public decimal BudgetAmount { get; set; }
        public decimal Amount { get; set; }

        public string Name {  get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public TransactionType TransactionType { get; set; }

        public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.Now);

        public string Category { get; set; } = string.Empty;

    }
}
