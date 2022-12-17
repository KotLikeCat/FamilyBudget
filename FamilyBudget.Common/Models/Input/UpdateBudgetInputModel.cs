using System.ComponentModel.DataAnnotations;
#pragma warning disable CS8618

namespace FamilyBudget.Common.Models.Input;

public class UpdateBudgetInputModel
{
    [Required]
    public string Name { get; set; }
    public string? Description { get; set; }
    public List<Guid> UserIds { get; set; }
}