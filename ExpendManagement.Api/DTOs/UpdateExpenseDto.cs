using System.Reflection.Metadata.Ecma335;
using ExpenseManagement.Api.Enums;

namespace ExpenseManagement.Api.DTOs
{
    public class UpdateExpenseDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public DateTime ExpenseDate { get; set; }
        public ExpenseCategory Category { get; set; }
    }
}
