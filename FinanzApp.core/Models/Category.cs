using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanzApp.core.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public TransactionType AppliesTo { get; set; } = TransactionType.None;
        public string? ColorHex { get; set; }
        public decimal? MonthlyBudget { get; set; }
        public bool IsArchived { get; set; }
    }


}
