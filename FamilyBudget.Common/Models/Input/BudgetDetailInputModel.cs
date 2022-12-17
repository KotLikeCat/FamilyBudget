namespace FamilyBudget.Common.Models.Input;
#pragma warning disable CS8618

public class BudgetDetailInputModel
{
    public double Amount { get; set; }
    public Guid CategoryId { get; set; }
    public string? Description { get; set; }
    public bool IsIncome { get; set; }
}