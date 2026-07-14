using ExpenseManagement.Api.DTOs;

public interface IExpenseService
{
    Task<List<ExpenseResponseDto>> GetAllAsync();
    Task<ExpenseResponseDto?> GetByIdAsync(int id);
    Task CreateAsync(CreateExpenseDto dto);
    Task<bool> UpdateAsync(int id, UpdateExpenseDto dto);
    Task<bool> DeleteAsync(int id);
    Task<bool> RejectAsync(int id, string managerNote);
    Task<bool> ApproveAsync(int id);
}