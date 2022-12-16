using AutoMapper;
using FamilyBudget.Api.Authorization;
using FamilyBudget.Common.FilterPipelines.Budget;
using FamilyBudget.Common.Models.Data;
using FamilyBudget.Common.Models.Input;
using FamilyBudget.Common.Models.View;
using FamilyBudget.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FamilyBudget.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class BudgetsController : ControllerBase
{
    private readonly DatabaseContext _context;
    private readonly IMapper _mapper;

    public BudgetsController(DatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetBudget(Guid id)
    {
        var budget = await _context.Budgets.Include(x => x.BudgetUsers).SingleOrDefaultAsync(x => x.Id == id);
        if (budget == null)
        {
            return NotFound();
        }

        var result = _mapper.Map<BudgetViewModel>(budget);
        return Ok(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateBudget(Guid id, [FromBody] UpdateBudgetInputModel input)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var budget = await _context.Budgets.Include(x => x.BudgetUsers).SingleOrDefaultAsync(x => x.Id == id);
        if (budget == null)
        {
            return NotFound();
        }

        budget.Name = input.Name;
        budget.Description = input.Description;
        budget.BudgetUsers.Clear();
        input.UserIds.ForEach(x=>budget.BudgetUsers.Add(new BudgetUser
        {
            BudgetId = id,
            UserId = x
        }));

        await _context.SaveChangesAsync();

        var result = _mapper.Map<BudgetViewModel>(budget);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateBudget([FromBody] BudgetInputModel input)
    {
        if (await _context.Budgets.AnyAsync(x => x.Name == input.Name))
        {
            return BadRequest(new ErrorViewModel("A budget with the same name already exists"));
        }

        var user = (HttpContext.Items["User"] as User)!;
        var entity = _mapper.Map<Budget>(input);
        entity.UserId = user.Id;

        await _context.Budgets.AddAsync(entity);
        await _context.SaveChangesAsync();

        var view = _mapper.Map<BudgetViewModel>(entity);
        return Ok(view);
    }

    [HttpGet]
    public async Task<IActionResult> GetBudgets([FromQuery] FilterInputModel input)
    {
        var user = (HttpContext.Items["User"] as User)!;
        var query = _context.Budgets
            .Include(x => x.BudgetUsers)
            .Include(x => x.User)
            .Where(x => x.BudgetUsers.Any(y => y.UserId == user.Id) || x.UserId == user.Id);

        var pipeline = new BudgetFilterPipeline(query);
        var budgets = await pipeline.ExecuteAsync(input);
        var budgetsView = _mapper.Map<BaseListViewModel<BudgetViewModel>>(budgets);
        return Ok(budgetsView);
    }
}