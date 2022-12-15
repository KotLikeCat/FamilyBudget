using AutoMapper;
using FamilyBudget.Api.Cache;
using FamilyBudget.Common.Models.Data;
using FamilyBudget.Common.Models.Input;
using FamilyBudget.Common.Models.View;
using FamilyBudget.Common.Tools;
using FamilyBudget.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FamilyBudget.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly DatabaseContext _context;
    private readonly IMapper _mapper;
    private readonly IJwtUtils _jwtUtils;
    private readonly IUserCache _userCache;

    public AuthenticationController(DatabaseContext context, IJwtUtils jwtUtils, IMapper mapper, IUserCache userCache)
    {
        _context = context;
        _jwtUtils = jwtUtils;
        _mapper = mapper;
        _userCache = userCache;
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] UserInputModel input)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var hashedPassword = PasswordTools.HashPassword(input.Password);
        
        var user = await _context.Users.SingleOrDefaultAsync(x => x.Login == input.Login && x.Password == hashedPassword);

        switch (user)
        {
            // We will create an account if there are no accounts
            case null when !await _context.Users.AnyAsync():
            {
                var entity = _mapper.Map<User>(input);
                entity.Password = hashedPassword;
                
                var result = await _context.Users.AddAsync(entity);
                user = result.Entity;
                
                break;
            }
            case null:
                return Unauthorized(new ErrorViewModel("Wrong login or password"));
        }

        var token = _jwtUtils.GenerateJwtToken(user);
        
        user.LastLoginTime = DateTime.UtcNow;
        user.AuthenticationHash = PasswordTools.HashPassword(token);
        
        await _context.SaveChangesAsync();
        
        _userCache.FlushCacheByUserId(user.Id);
        
        return Ok(new JwtTokenViewModel(token));
    }
}