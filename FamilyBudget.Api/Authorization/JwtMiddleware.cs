using FamilyBudget.Api.Cache;
using FamilyBudget.Common.Tools;

namespace FamilyBudget.Api.Authorization;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;

    public JwtMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, IJwtUtils jwtUtils, IUserCache userCache)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        var userId = jwtUtils.ValidateJwtToken(token);
        if (userId == null)
        {
            await _next(context);
            return;
        }
        
        var user = userCache.GetUser(userId.Value);
        var authenticationHash = PasswordTools.HashPassword(token!);
        
        if (user?.AuthenticationHash == authenticationHash)
        {
            context.Items["User"] = user;
        }
        await _next(context);
    }
}