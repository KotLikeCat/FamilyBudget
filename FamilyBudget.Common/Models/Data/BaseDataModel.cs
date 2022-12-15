using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FamilyBudget.Common.Models.Data;

public abstract class BaseDataModel
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Index { get; set; }
    
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
}