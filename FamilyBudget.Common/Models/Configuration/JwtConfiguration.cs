namespace FamilyBudget.Common.Models.Configuration;

public class JwtConfiguration
{
    public JwtConfiguration(string secret)
    {
        Secret = secret;
    }

    public string Secret { get; set; }
}