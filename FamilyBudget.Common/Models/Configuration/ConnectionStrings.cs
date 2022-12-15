namespace FamilyBudget.Common.Models.Configuration;

public class ConnectionStrings
{
    public ConnectionStrings(string local, string container)
    {
        Local = local;
        Container = container;
    }

    public string Local { get; set; }
    public string Container { get; set; }
}