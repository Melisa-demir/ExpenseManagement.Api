using ExpenseManagement.Api.DTOs;
using ExpenseManagement.Api.Entitites;
using ExpenseManagement.Api.Enums;
using ExpenseManagement.Api.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
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

        public async Task<bool> UpdateAsync(int id, UpdateExpenseDto dto)
        {
            var expense = await _expenseRepository.GetByIdAsync(id);

            if(expense == null)
            {
                return false;
            }

            if(expense.Status != ExpenseStatus.Pending)
            {
                return false;
            }

            expense.Title = dto.Title;
            expense.Description = dto.Description;
            expense.Amount = dto.Amount;
            expense.ExpenseDate = dto.ExpenseDate;
            expense.Category = dto.Category;
            expense.UpdatedAt = DateTime.UtcNow;

            _expenseRepository.Update(expense);

            return await _expenseRepository.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var expense = await _expenseRepository.GetByIdAsync(id);

            if (expense == null)
                return false;

            if (expense.Status != ExpenseStatus.Pending)
            {
                return false;
            }

            _expenseRepository.Delete(expense);

            return await _expenseRepository.SaveChangesAsync();
        }

        public async Task<bool> ApproveAsync(int id)
        {
            var expense = await _expenseRepository.GetByIdAsync(id);

            if (expense == null)
                return false;

            if (expense.Status != ExpenseStatus.Pending)
                return false;

            expense.Status = ExpenseStatus.Approved;
            expense.ManagerNote = null;
            expense.UpdatedAt = DateTime.UtcNow;

            _expenseRepository.Update(expense);

            return await _expenseRepository.SaveChangesAsync();
        }

        public async Task<bool> RejectAsync(int id, string managerNote)
        {
            var expense = await _expenseRepository.GetByIdAsync(id);

            if (expense == null)
                return false;

            if (expense.Status != ExpenseStatus.Pending)
                return false;

            if (string.IsNullOrWhiteSpace(managerNote))
                return false;

            expense.Status = ExpenseStatus.Rejected;
            expense.ManagerNote = managerNote;
            expense.UpdatedAt = DateTime.UtcNow;

            _expenseRepository.Update(expense);

            return await _expenseRepository.SaveChangesAsync();
        }
    }
}

