namespace FamilyBudget.Common.Models.View;

public class JwtTokenViewModel
{
    public JwtTokenViewModel(string token)
    {
        Token = token;
    }

    public string Token { get; set; }
}