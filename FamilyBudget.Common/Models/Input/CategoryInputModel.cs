using System.ComponentModel.DataAnnotations;

namespace FamilyBudget.Common.Models.Input;

public class CategoryInputModel
{
    public CategoryInputModel(string name)
    {
        Name = name;
    }

    [Required, MaxLength(50)]
    public string Name { get; set; }
}