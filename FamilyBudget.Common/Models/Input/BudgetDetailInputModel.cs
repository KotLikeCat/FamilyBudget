namespace FamilyBudget.Common.Models.Input;

public class BudgetDetailInputModel
{
    public double Amount { get; set; }
    public Guid CategoryId { get; set; }
    public string? Description { get; set; }
    public bool IsIncome { get; set; }
}