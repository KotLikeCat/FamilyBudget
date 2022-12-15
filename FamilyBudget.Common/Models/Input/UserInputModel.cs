using System.ComponentModel.DataAnnotations;

namespace FamilyBudget.Common.Models.Input;

public class UserInputModel
{
    [Required]
    public string Login { get; set; }
    
    [Required]
    public string Password { get; set; }
}