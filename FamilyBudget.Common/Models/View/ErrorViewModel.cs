namespace FamilyBudget.Common.Models.View;

public class ErrorViewModel
{
    public ErrorViewModel(string error)
    {
        Message = error;
    }

    public string Message { get; set; }
}