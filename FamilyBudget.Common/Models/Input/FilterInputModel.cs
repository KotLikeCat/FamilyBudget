using System.ComponentModel.DataAnnotations;

namespace FamilyBudget.Common.Models.Input;

public class FilterInputModel
{
    [Required] public string Sort { get; set; }
    [Required] public string Range { get; set; }
    [Required] public string Filter { get; set; }
}