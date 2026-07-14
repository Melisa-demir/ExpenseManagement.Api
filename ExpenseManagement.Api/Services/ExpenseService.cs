using System.Text.Json;
using ExpenseManagement.Api.DTOs;
using ExpenseManagement.Api.Entitites;
using ExpenseManagement.Api.Enums;
using ExpenseManagement.Api.Repositories;
using Microsoft.Extensions.Caching.Distributed;

namespace ExpenseManagement.Api.Services
{
    public class ExpenseService : IExpenseService
    {
        private const string CacheKey = "expenses";

        private readonly IExpenseRepository _expenseRepository;
        private readonly IDistributedCache _cache;

        public ExpenseService(
            IExpenseRepository expenseRepository,
            IDistributedCache cache)
        {
            _expenseRepository = expenseRepository;
            _cache = cache;
        }

        public async Task<List<ExpenseResponseDto>> GetAllAsync()
        {
            var cachedExpenses = await _cache.GetStringAsync(CacheKey);

            if (!string.IsNullOrEmpty(cachedExpenses))
            {
                return JsonSerializer.Deserialize<List<ExpenseResponseDto>>(
                    cachedExpenses)!;
            }

            var expenses = await _expenseRepository.GetAllAsync();

            var response = expenses.Select(x => new ExpenseResponseDto
            {
                Id = x.Id,
                UserId = x.UserId,
                Title = x.Title,
                Description = x.Description,
                Amount = x.Amount,
                ExpenseDate = x.ExpenseDate,
                Category = x.Category,
                Status = x.Status,
                ManagerNote = x.ManagerNote,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            }).ToList();

            await _cache.SetStringAsync(
                CacheKey,
                JsonSerializer.Serialize(response),
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                });

            return response;
        }

        public async Task<ExpenseResponseDto?> GetByIdAsync(int id)
        {
            var expense = await _expenseRepository.GetByIdAsync(id);

            if (expense == null)
                return null;

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

            await _cache.RemoveAsync(CacheKey);
        }

        public async Task<bool> UpdateAsync(int id, UpdateExpenseDto dto)
        {
            var expense = await _expenseRepository.GetByIdAsync(id);

            if (expense == null)
                return false;

            expense.Title = dto.Title;
            expense.Description = dto.Description;
            expense.Amount = dto.Amount;
            expense.ExpenseDate = dto.ExpenseDate;
            expense.Category = dto.Category;
            expense.UpdatedAt = DateTime.UtcNow;

            _expenseRepository.Update(expense);

            var result = await _expenseRepository.SaveChangesAsync();

            if (result)
                await _cache.RemoveAsync(CacheKey);

            return result;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var expense = await _expenseRepository.GetByIdAsync(id);

            if (expense == null)
                return false;

            _expenseRepository.Delete(expense);

            var result = await _expenseRepository.SaveChangesAsync();

            if (result)
                await _cache.RemoveAsync(CacheKey);

            return result;
        }

        public async Task<bool> ApproveAsync(int id)
        {
            var expense = await _expenseRepository.GetByIdAsync(id);

            if (expense == null || expense.Status != ExpenseStatus.Pending)
                return false;

            expense.Status = ExpenseStatus.Approved;
            expense.ManagerNote = null;
            expense.UpdatedAt = DateTime.UtcNow;

            _expenseRepository.Update(expense);

            var result = await _expenseRepository.SaveChangesAsync();

            if (result)
                await _cache.RemoveAsync(CacheKey);

            return result;
        }

        public async Task<bool> RejectAsync(int id, string managerNote)
        {
            var expense = await _expenseRepository.GetByIdAsync(id);

            if (expense == null ||
                expense.Status != ExpenseStatus.Pending ||
                string.IsNullOrWhiteSpace(managerNote))
            {
                return false;
            }

            expense.Status = ExpenseStatus.Rejected;
            expense.ManagerNote = managerNote;
            expense.UpdatedAt = DateTime.UtcNow;

            _expenseRepository.Update(expense);

            var result = await _expenseRepository.SaveChangesAsync();

            if (result)
                await _cache.RemoveAsync(CacheKey);

            return result;
        }
    }
}