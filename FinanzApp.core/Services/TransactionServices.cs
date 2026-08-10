using FinanzApp.core.Infrastructure;
using FinanzApp.core.Interfaces;
using FinanzApp.core.Models;
using Microsoft.EntityFrameworkCore;

namespace FinanzApp.core.Services
{
    public class TransactionServices : ITransactionService
    {
        #region Constructor
        public TransactionServices(FinanzAppDbContext context) 
        {
            _dbcontext = context;
        }

        #endregion

        #region Fields
        private readonly FinanzAppDbContext _dbcontext;

        #endregion

        #region Methods
        public async Task AddAsync(Transaction transaction)
        {
            _dbcontext.Transactions.Add(transaction);
            await _dbcontext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Transaction transaction)
        {
            _dbcontext.Transactions.Remove(transaction);
            await _dbcontext.SaveChangesAsync();
        }

        public async Task<List<Transaction>> GetAllAsync()
        {
            return await _dbcontext.Transactions
                .Include(t => t.Category)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Transaction?> GetByIdAsync(int id)
        {
            return await _dbcontext.Transactions.FindAsync(id);
        }

        public async Task UpdateAsync(Transaction updatedTransaction)
        {
            var trackedTransaction = _dbcontext.Transactions.Local.FirstOrDefault(t => t.Id == updatedTransaction.Id) ?? 
                await _dbcontext.Transactions.FirstOrDefaultAsync(t => t.Id == updatedTransaction.Id);

            if (trackedTransaction is null)
                throw new InvalidOperationException($"Transaktion {updatedTransaction.Id} wurde nicht gefunden.");

            if (!ReferenceEquals(trackedTransaction, updatedTransaction))
            {
                trackedTransaction.Name = updatedTransaction.Name; 
                trackedTransaction.Description = updatedTransaction.Description;
                trackedTransaction.Date = updatedTransaction.Date;
                trackedTransaction.Amount = updatedTransaction.Amount;
                trackedTransaction.BudgetAmount = updatedTransaction.BudgetAmount;
                trackedTransaction.TransactionType = updatedTransaction.TransactionType;
                trackedTransaction.CategoryId= updatedTransaction.CategoryId;
            }

            await _dbcontext.SaveChangesAsync();
        }

        #endregion
    }
}
