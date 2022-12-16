using System.ComponentModel.DataAnnotations;

namespace FamilyBudget.Common.Models.Input;

public class BudgetInputModel
{
    public BudgetInputModel(string name, string? description, List<Guid>? userIds)
    {
        Name = name;
        Description = description;
        UserIds = userIds;
    }

    [Required]
    public string Name { get; set; }
    public string? Description { get; set; }
    public List<Guid>? UserIds { get; set; }
}