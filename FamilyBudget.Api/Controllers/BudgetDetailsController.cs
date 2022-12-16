using AutoMapper;
using FamilyBudget.Api.Authorization;
using FamilyBudget.Common.FilterPipelines.BudgetDetails;
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
public class BudgetDetailsController : ControllerBase
{
    private readonly DatabaseContext _context;
    private readonly IMapper _mapper;

    public BudgetDetailsController(DatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpPost("{budgetId:guid}")]
    public async Task<IActionResult> CreateBudgetDetail(Guid budgetId, [FromBody] BudgetDetailInputModel input)
    {
        var user = (HttpContext.Items["User"] as User)!;
        var entity = _mapper.Map<BudgetDetail>(input);
        entity.BudgetId = budgetId;
        entity.UserId = user.Id;

        await _context.BudgetDetails.AddAsync(entity);
        await _context.SaveChangesAsync();

        var view = _mapper.Map<BudgetDetailViewModel>(entity);
        return Ok(view);
    }

    [HttpGet("{budgetId:guid}")]
    public async Task<IActionResult> GetBudgetDetails(Guid budgetId, [FromQuery] FilterInputModel input)
    {
        var query = _context.BudgetDetails
            .Where(x => x.BudgetId == budgetId)
            .Include(x => x.User)
            .Include(x => x.Category);
        var pipeline = new BudgetDetailsFilterPipeline(query);
        var budgets = await pipeline.ExecuteAsync(input);
        var budgetsView = _mapper.Map<BaseListViewModel<BudgetDetailViewModel>>(budgets);
        return Ok(budgetsView);
    }

    [HttpGet("{budgetId:guid}/{detailId:guid}")]
    public async Task<IActionResult> GetBudgetDetail(Guid budgetId, Guid detailId)
    {
        var detail = await _context.BudgetDetails
            .AsNoTracking()
            .Include(x => x.User)
            .Include(x => x.Category)
            .SingleOrDefaultAsync(x => x.BudgetId == budgetId && x.Id == detailId);

        if (detail == null)
        {
            return NotFound();
        }

        var view = _mapper.Map<BudgetDetailViewModel>(detail);
        return Ok(view);
    }

    [HttpPut("{budgetId:guid}/{detailId:guid}")]
    public async Task<IActionResult> UpdateBudgetDetail(Guid budgetId, Guid detailId,
        [FromBody] BudgetDetailInputModel input)
    {
        var detail = await _context.BudgetDetails
            .SingleOrDefaultAsync(x => x.BudgetId == budgetId && x.Id == detailId);

        if (detail == null)
        {
            return NotFound();
        }

        detail.Amount = input.Amount;
        detail.CategoryId = input.CategoryId;
        detail.Description = input.Description;
        detail.IsIncome = input.IsIncome;
        await _context.SaveChangesAsync();

        var view = _mapper.Map<BudgetDetailViewModel>(detail);
        return Ok(view);
    }

    [HttpDelete("{budgetId:guid}/{detailId:guid}")]
    public async Task<IActionResult> DeleteBudgetDetail(Guid budgetId, Guid detailId)
    {
        var detail = await _context.BudgetDetails
            .SingleOrDefaultAsync(x => x.BudgetId == budgetId && x.Id == detailId);

        if (detail == null)
        {
            return NotFound();
        }

        _context.BudgetDetails.Remove(detail);
        await _context.SaveChangesAsync();

        var view = _mapper.Map<BudgetDetailViewModel>(detail);
        return Ok(view);
    }

    [HttpDelete("{budgetId:guid}")]
    public async Task<IActionResult> DeleteBudgetDetails(Guid budgetId, List<Guid> list)
    {
        var details = _context.BudgetDetails.Where(x => x.BudgetId == budgetId && list.Any(y => x.Id == y)).ToList();

        if (details.Count != list.Count)
        {
            return BadRequest(new ErrorViewModel("Some of budget details are not exist"));
        }

        _context.BudgetDetails.RemoveRange(details);
        await _context.SaveChangesAsync();

        var view = _mapper.Map<List<BudgetDetailViewModel>>(details);
        return Ok(view);
    }
}