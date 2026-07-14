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
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(x => x.Username)
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }

}
