using System.ComponentModel.DataAnnotations.Schema;

namespace FamilyBudget.Common.Models.Data;

[Table("budget_details")]
public class BudgetDetail : BaseDataModel
{
    [Column("budget_id")]
    [ForeignKey("Budget")]
    public Guid BudgetId { get; set; }
    
    public double Amount { get; set; }
    public string? Description { get; set; }
    
    [Column("user_id")]
    [ForeignKey("User")]
    public Guid UserId { get; set; }
    
    [Column("category_id")]
    [ForeignKey("Category")]
    public Guid CategoryId { get; set; }
    
    [Column("is_income")]
    public bool IsIncome { get; set; }
    
    public Budget Budget { get; set; }
    public User User { get; set; }
    public Category Category { get; set; }
}