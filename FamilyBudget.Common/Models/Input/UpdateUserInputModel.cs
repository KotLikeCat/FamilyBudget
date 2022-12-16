using System.ComponentModel.DataAnnotations;

namespace FamilyBudget.Common.Models.Input;

public class UpdateUserInputModel
{
    public UpdateUserInputModel(string password)
    {
        Password = password;
    }

    [Required]
    [MinLength(6)]
    public string Password { get; set; }
}