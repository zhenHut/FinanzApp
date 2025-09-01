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
            return await _dbcontext.Transactions.ToListAsync();
        }

        public async Task<Transaction?> GetByIdAsync(int id)
        {
            return await _dbcontext.Transactions.FindAsync(id);
        }

        public async Task UpdateAsync(Transaction transaction)
        {
            _dbcontext.Transactions.Update(transaction);
            await _dbcontext.SaveChangesAsync();
        }

        #endregion
    }
}
