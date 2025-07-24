using FinanzApp.core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanzApp.core.Interface
{
    public interface ITransactionService
    {
        Task<List<Transaction>> GetAllAsync();
        Task <Transaction?> GetByIdAsync(int id);
        Task AddAsync(Transaction transaction);
        Task DeletAsync(Transaction transaction);
        Task UpdateAsync(Transaction transaction);
    }
}
