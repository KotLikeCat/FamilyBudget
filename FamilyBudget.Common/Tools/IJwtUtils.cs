using FamilyBudget.Common.Models.Data;

namespace FamilyBudget.Common.Tools;

public interface IJwtUtils
{
    string GenerateJwtToken(User user);
    Guid? ValidateJwtToken(string? token);
}