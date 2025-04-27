using Microsoft.EntityFrameworkCore;
using MonthlyExpenseTracker.EntityModels;

namespace MonthlyExpenseTracker.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { } 

        public DbSet<Category> Categories => Set<Category>();   
        public DbSet<Expense> Expenses => Set<Expense>();   

    }
}
