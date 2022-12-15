namespace FamilyBudget.Common.Models.Data;

public class User : BaseDataModel
{
    public User(string login, string password)
    {
        Login = login;
        Password = password;
        CreateTime = DateTime.UtcNow;
    }

    public string Login { get; set; }
    public string Password { get; set; }
    public DateTime CreateTime { get; set; }
    public DateTime? LastLoginTime { get; set; }
    public string? AuthenticationHash { get; set; }
}