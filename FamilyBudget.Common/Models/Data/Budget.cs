using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FamilyBudget.Common.Models.Data;

public class Budget : BaseDataModel
{
    public Budget(string name, string? description)
    {
        Name = name;
        CreateTime = DateTime.UtcNow;
        Description = description;
    }

    [MaxLength(50)]
    public string Name { get; set; }
    
    [MaxLength(250)]
    public string? Description { get; set; }

    [Column("create_time")]
    public DateTime CreateTime { get; set; }
    
    [Column("user_id")]
    [ForeignKey("User")]
    public Guid UserId { get; set; }

    public List<BudgetUser> BudgetUsers { get; set; }
    public User User { get; set; }
}