using System.ComponentModel.DataAnnotations.Schema;

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
    
    [Column("create_time")]
    public DateTime CreateTime { get; set; }
    
    [Column("last_login_time")]
    public DateTime? LastLoginTime { get; set; }
    
    [Column("authentication_hash")]
    public string? AuthenticationHash { get; set; }
    
    public List<BudgetUser> BudgetUsers { get; set; }
}