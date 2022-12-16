using System.ComponentModel.DataAnnotations.Schema;

namespace FamilyBudget.Common.Models.Data;

[Table("budgets_users")]
public class BudgetUser : BaseDataModel
{
    [Column("budget_id")]
    [ForeignKey("Budget")]
    public Guid BudgetId { get; set; }
    
    [Column("user_id")]
    [ForeignKey("User")]
    public Guid UserId { get; set; }
    
    public Budget Budget { get; set; }
    public User User { get; set; }
}