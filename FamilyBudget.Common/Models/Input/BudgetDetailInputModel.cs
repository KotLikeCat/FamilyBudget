using System.ComponentModel.DataAnnotations;

namespace FamilyBudget.Common.Models.Input;
#pragma warning disable CS8618

public class BudgetDetailInputModel
{
    [Required]
    public double Amount { get; set; }
    
    [Required]
    public Guid CategoryId { get; set; }
    public string? Description { get; set; }
    
    [Required]
    public bool IsIncome { get; set; }
}