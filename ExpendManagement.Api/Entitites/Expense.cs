using System.Security.Principal;
using ExpenseManagement.Api.Enums;

namespace ExpenseManagement.Api.Entitites
{
    public class Expense
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public DateTime ExpenseDate { get; set; }
        public ExpenseCategory Category { get; set; }
        public ExpenseStatus Status { get; set; }
        public string ManagerNote { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
