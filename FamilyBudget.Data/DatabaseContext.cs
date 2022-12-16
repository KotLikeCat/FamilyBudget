using FamilyBudget.Common.Models.Data;
using Microsoft.EntityFrameworkCore;
#pragma warning disable CS8618

namespace FamilyBudget.Data;

public class DatabaseContext : Microsoft.EntityFrameworkCore.DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
        
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Budget> Budgets { get; set; }
    public DbSet<BudgetUser> BudgetUsers { get; set; }
    public DbSet<Category> Categories { get; set; }
}