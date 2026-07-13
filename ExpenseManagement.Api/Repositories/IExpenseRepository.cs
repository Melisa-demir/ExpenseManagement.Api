using ExpenseManagement.Api.Entitites;

namespace ExpenseManagement.Api.Repositories
{
    public interface IExpenseRepository
    {
        Task<List<Expense>> GetAllAsync();
        Task<Expense?> GetByIdAsync(int id);
        Task<List<Expense>> GetByUserIdAsync(int userId);
        Task AddAsync(Expense expense);
        void Update(Expense expense);
        void Delete(Expense expense);
        Task<bool> SaveChangeAsync();
    }
}
