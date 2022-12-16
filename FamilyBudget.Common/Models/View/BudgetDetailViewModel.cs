namespace FamilyBudget.Common.Models.View;

public class BudgetDetailViewModel
{
    public Guid Id { get; set; }
    public string Owner { get; set; }
    public string? Description { get; set; }
    public string Category { get; set; }
    public Guid CategoryId { get; set; }
    public double Amount { get; set; }
    public bool IsIncome { get; set; }
}