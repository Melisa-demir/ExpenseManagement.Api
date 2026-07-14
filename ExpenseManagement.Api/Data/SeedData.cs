using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ExpenseManagement.Api.Entitites;

namespace ExpenseManagement.Api.Data
{
    public static class SeedData
    {
        public static async Task InitialAsync(ExpenseDbContext context)
        {
            await context.Database.MigrateAsync();

            bool AdminExists = await context.Users
                .AnyAsync(x => x.Role == "Admin");

            if (AdminExists)
                return;

            var admin = new User
            {
                Username = "admin",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                Role = "Admin"
            };

            context.Users.Add(admin);
            await context.SaveChangesAsync();
        }
    }
}
