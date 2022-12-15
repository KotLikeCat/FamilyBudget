#pragma warning disable CS8618
namespace FamilyBudget.Common.Models.View;

public class UserViewModel
{
    public Guid Id { get; set; }
    public string Login { get; set; }
    public string CreatedAt { get; set; }
    public string? LastLoginAt { get; set; }
}