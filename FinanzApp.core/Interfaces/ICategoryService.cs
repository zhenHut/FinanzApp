using FinanzApp.core.Models;


namespace FinanzApp.core.Interfaces
{
    public interface ICategoryService
    {
        Task<IReadOnlyList<Category>> GetAllSync();
        Task<Category> AddAsync(Category category);
        Task UpdateAsync(Category category);
    }
}
