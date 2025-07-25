using FinanzApp.core.Data;
using FinanzApp.core.Interface;
using FinanzApp.core.Model;
using Microsoft.EntityFrameworkCore;

namespace FinanzApp.core.Services
{
    public class TransactionServices : ITransactionService
    {
        #region Constructor
        public TransactionServices(FinanzAppDbContext context) 
        {
            _context = context;
        }

        #endregion

        #region Fields
        private readonly FinanzAppDbContext _context;

        #endregion

        #region Methods
        public async Task AddAsync(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Transaction transaction)
        {
            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Transaction>> GetAllAsync()
        {
            return await _context.Transactions.ToListAsync();
        }

        public async Task<Transaction?> GetByIdAsync(int id)
        {
            return await _context.Transactions.FindAsync(id);
        }

        public async Task UpdateAsync(Transaction transaction)
        {
            _context.Transactions.Update(transaction);
            await _context.SaveChangesAsync();
        }

        #endregion
    }
}
