using FinanzApp.core.Infrastructure;
using FinanzApp.core.Interfaces;
using FinanzApp.core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanzApp.core.Services
{
    public class CategoryService : ICategoryService
    {
        #region Constructor

        public CategoryService(FinanzAppDbContext context) 
        {
            _context = context;
        }

        #endregion

        #region Fields

        private readonly FinanzAppDbContext _context;


        #endregion

        #region Methods
        public async Task<IReadOnlyList<Category>> GetAllSync()
        {
            return await _context.Categories.AsNoTracking().OrderBy(c => c.Name).ToListAsync();
        }

        public async Task<Category> AddSync(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task UpdateAsync(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }

        #endregion
    }
}
