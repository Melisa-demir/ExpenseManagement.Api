using ExpenseManagement.Api.DTOs;
using ExpenseManagement.Api.Entitites;
using ExpenseManagement.Api.Enums;
using ExpenseManagement.Api.Repositories;
using Microsoft.Identity.Client;

namespace ExpenseManagement.Api.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly IExpenseRepository _expenseRepository;
        public ExpenseService(IExpenseRepository expenseRepository)
        {
            _expenseRepository = expenseRepository;
        }

        public async Task<List<ExpenseResponseDto>> GetAllAsync()
        {
            var expenses = await _expenseRepository.GetAllAsync();

            return expenses.Select(expense => new ExpenseResponseDto
            {
                Id = expense.Id,
                UserId = expense.UserId,
                Title = expense.Title,
                Description = expense.Description,
                Amount = expense.Amount,
                ExpenseDate = expense.ExpenseDate,
                Category = expense.Category,
                Status = expense.Status,
                ManagerNote = expense.ManagerNote,
                CreatedAt = expense.CreatedAt,
                UpdatedAt = expense.UpdatedAt
            }).ToList();
        }
        public async Task<ExpenseResponseDto> GetByIdAsync(int id)
        {
            var expense = await _expenseRepository.GetByIdAsync(id);

            if (expense == null)
            {
                return null;
            }

            return new ExpenseResponseDto
            {
                Id = expense.Id,
                UserId = expense.UserId,
                Title = expense.Title,
                Description = expense.Description,
                Amount = expense.Amount,
                ExpenseDate = expense.ExpenseDate,
                Category = expense.Category,
                Status = expense.Status,
                ManagerNote = expense.ManagerNote,
                CreatedAt = expense.CreatedAt,
                UpdatedAt = expense.UpdatedAt
            };
        }
        public async Task CreateAsync(CreateExpenseDto dto)
        {
            var expense = new Expense
            {
                UserId = dto.UserId,
                Title = dto.Title,
                Description = dto.Description,
                Amount = dto.Amount,
                ExpenseDate = dto.ExpenseDate,
                Category = dto.Category,
                Status = ExpenseStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            await _expenseRepository.AddAsync(expense);

            await _expenseRepository.SaveChangesAsync();
        }
    }
}

