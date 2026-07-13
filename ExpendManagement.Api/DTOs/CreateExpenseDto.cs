using ExpenseManagement.Api.Enums;

namespace ExpenseManagement.Api.DTOs
{
    public class CreateExpenseDto
    {
        public int UserId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public DateTime ExpenseDate { get; set; }
        public ExpenseCategory Category { get; set; }

    }
}
