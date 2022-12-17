using System.ComponentModel.DataAnnotations;
#pragma warning disable CS8618

namespace FamilyBudget.Common.Models.Input;

public class UpdateUserInputModel
{
    [Required]
    [MinLength(6)]
    public string Password { get; set; }
}