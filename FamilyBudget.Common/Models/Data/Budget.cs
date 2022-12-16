using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FamilyBudget.Common.Models.Data;

public class Budget : BaseDataModel
{
    public Budget(string name, DateTime createTime, string? description)
    {
        Name = name;
        CreateTime = createTime;
        Description = description;
    }

    [MaxLength(50)]
    public string Name { get; set; }
    
    [MaxLength(250)]
    public string? Description { get; set; }
    
    [Column("user_id")]
    public Guid UserId { get; set; }
    
    [Column("create_time")]
    public DateTime CreateTime { get; set; }
}