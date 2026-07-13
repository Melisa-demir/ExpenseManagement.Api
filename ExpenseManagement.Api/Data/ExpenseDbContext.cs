using System.Data.Common;
using ExpenseManagement.Api.Entitites;
using Microsoft.EntityFrameworkCore;

namespace ExpenseManagement.Api.Data
{
    public class ExpenseDbContext : DbContext
    {
        public ExpenseDbContext(DbContextOptions<ExpenseDbContext> options) : base(options)
        {
        }

        public DbSet<Expense> Expenses { get; set; }
    }
}
