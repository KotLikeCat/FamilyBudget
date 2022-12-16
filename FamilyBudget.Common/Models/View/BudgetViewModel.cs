#pragma warning disable CS8618
namespace FamilyBudget.Common.Models.View;

public class BudgetViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Owner { get; set; }
    public string CreateTime { get; set; }
    public List<Guid> UserIds { get; set; }
}