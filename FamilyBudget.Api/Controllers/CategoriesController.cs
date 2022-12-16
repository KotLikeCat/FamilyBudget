using AutoMapper;
using FamilyBudget.Api.Authorization;
using FamilyBudget.Common.FilterPipelines.Category;
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
public class CategoriesController : ControllerBase
{
    private readonly DatabaseContext _context;
    private readonly IMapper _mapper;

    public CategoriesController(DatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetCategories([FromQuery] FilterInputModel input)
    {
        var pipeline = new CategoryFilterPipeline(_context.Categories.AsNoTracking().AsQueryable());
        var categories = await pipeline.ExecuteAsync(input);
        var view = _mapper.Map<BaseListViewModel<CategoryViewModel>>(categories);

        return Ok(view);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetCategory(Guid id)
    {
        var category = await _context.Categories.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);
        if (category == null)
        {
            return NotFound();
        }

        var view = _mapper.Map<CategoryViewModel>(category);
        return Ok(view);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory([FromBody] CategoryInputModel input)
    {
        if (await _context.Categories.AnyAsync(x => x.Name == input.Name))
        {
            return BadRequest(new ErrorViewModel("Category with the same name already exists"));
        }

        var entity = _mapper.Map<Category>(input);
        await _context.Categories.AddAsync(entity);
        await _context.SaveChangesAsync();

        var view = _mapper.Map<CategoryViewModel>(entity);
        return Ok(view);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateCategory(Guid id, [FromBody] CategoryInputModel input)
    {
        var category = await _context.Categories.SingleOrDefaultAsync(x => x.Id == id);
        if (category == null)
        {
            return NotFound();
        }
        
        category.Name = input.Name;
        await _context.SaveChangesAsync();

        var view = _mapper.Map<CategoryViewModel>(category);
        return Ok(view);
    }
}