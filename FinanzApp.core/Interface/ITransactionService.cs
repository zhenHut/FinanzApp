using FinanzApp.core.Model;

namespace FinanzApp.core.Interface
{
    public interface ITransactionService
    {
        Task<List<Transaction>> GetAllAsync();
        Task <Transaction?> GetByIdAsync(int id);
        Task AddAsync(Transaction transaction);
        Task DeleteAsync(Transaction transaction);
        Task UpdateAsync(Transaction transaction);
    }
}
