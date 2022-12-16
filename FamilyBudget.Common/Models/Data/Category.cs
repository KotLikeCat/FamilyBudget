using System.ComponentModel.DataAnnotations;

namespace FamilyBudget.Common.Models.Data;

public class Category : BaseDataModel
{
    public Category(string name)
    {
        Name = name;
    }

    [MaxLength(50)]
    public string Name { get; set; }
}