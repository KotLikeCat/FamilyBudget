using AutoMapper;
using FamilyBudget.Api.Authorization;
using FamilyBudget.Common.FilterPipelines.User;
using FamilyBudget.Common.Models.Data;
using FamilyBudget.Common.Models.Input;
using FamilyBudget.Common.Models.View;
using FamilyBudget.Common.Tools;
using FamilyBudget.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FamilyBudget.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly DatabaseContext _context;
    private readonly IMapper _mapper;

    public UsersController(DatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteUsers(List<Guid> list)
    {
        var users = await _context.Users.AsNoTracking().Where(x => list.Any(y => x.Id == y)).ToListAsync();
        if (users.Count != list.Count)
        {
            return BadRequest(new ErrorViewModel("Some of users are not exist"));
        }
        
        try
        {
            _context.Users.RemoveRange(users);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            return BadRequest(
                new ErrorViewModel("You have to delete all connections between users and any home budget elements"));
        }

        var result = _mapper.Map<List<UserViewModel>>(users);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        var user = await _context.Users.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);
        if (user == null)
        {
            return NotFound();
        }
        
        try
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            return BadRequest(
                new ErrorViewModel("You have to delete all connections between the user and any home budget elements"));
        }

        var result = _mapper.Map<UserViewModel>(user);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers([FromQuery] FilterInputModel input)
    {
        var currentUser = HttpContext.Items["User"] as User;
        var pipeline = new UserFilterPipeline(_context.Users.AsNoTracking().AsQueryable());
        var users = await pipeline.ExecuteAsync(input, currentUser!);
        var usersView = _mapper.Map<BaseListViewModel<UserViewModel>>(users);
        return Ok(usersView);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetUser(Guid id)
    {
        var user = await _context.Users.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);
        if (user == null)
        {
            return NotFound();
        }

        var result = _mapper.Map<UserViewModel>(user);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] UserInputModel input)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (await _context.Users.AnyAsync(x => x.Login == input.Login))
        {
            return BadRequest(new ErrorViewModel("User already exists in the database"));
        }
        
        var entity = _mapper.Map<UserInputModel, User>(input);
        entity.Password = PasswordTools.HashPassword(entity.Password);
        
        _context.Users.Add(entity);
        await _context.SaveChangesAsync();

        var result = _mapper.Map<User, UserViewModel>(entity);
        return Ok(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserInputModel input)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == id);
        if (user == null)
        {
            return NotFound();
        }

        user.Password = PasswordTools.HashPassword(input.Password);
        await _context.SaveChangesAsync();
        
        var result = _mapper.Map<UserViewModel>(user);
        return Ok(result);
    }
}