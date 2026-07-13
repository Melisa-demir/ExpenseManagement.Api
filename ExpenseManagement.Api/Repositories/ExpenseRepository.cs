using ExpenseManagement.Api.Data;
using ExpenseManagement.Api.Entitites;
using Microsoft.EntityFrameworkCore;

namespace ExpenseManagement.Api.Repositories
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly ExpenseDbContext _context;
        
        public ExpenseRepository(ExpenseDbContext context)
        {
            _context = context;
        }

        public async Task<List<Expense>> GetAllAsync()
        {
            return await _context.Expenses
                .AsNoTracking()
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task<Expense?> GetByIdAsync(int id)
        {
            return await _context.Expenses
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Expense>> GetByUserIdAsync(int userId)
        {
            return await _context.Expenses
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task AddAsync(Expense expense)
        {
            await _context.Expenses.AddAsync(expense);
        }

        public void Update(Expense expense)
        {
            _context.Expenses.Update(expense);
        }

        public void Delete(Expense expense)
        {
            _context.Expenses.Remove(expense);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

    }
}
