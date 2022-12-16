using FamilyBudget.Common.FilterPipeLines.Generic;
using FamilyBudget.Common.Models.Data;
using FamilyBudget.Common.Models.Input;
using FamilyBudget.Common.Models.View;
using Microsoft.EntityFrameworkCore;

namespace FamilyBudget.Common.FilterPipelines;

public abstract class BaseFilterPipeline<T> where T:BaseDataModel
{
    private IQueryable<T> _queryable;
    private readonly List<IPipelineFilter<T>> _pipelineFilters;
    private readonly IPipelineFilter<T> _paginationFilter;

    protected BaseFilterPipeline(IQueryable<T> queryable)
    {
        _queryable = queryable;
        _pipelineFilters = new List<IPipelineFilter<T>>();
        _paginationFilter = new Pagination<T>();
    }

    protected void AddPipelineFilter(IPipelineFilter<T> filter)
    {
        _pipelineFilters.Add(filter);
    }

    public async Task<BaseListViewModel<T>> ExecuteAsync(FilterInputModel input, params object[] dependencies)
    {
        var result = new BaseListViewModel<T>();
        
        _pipelineFilters.ForEach(x => _queryable = x.Execute(_queryable, input, dependencies));
        
        result.Length = await _queryable.CountAsync();
        result.Data = await _paginationFilter.Execute(_queryable, input, dependencies).ToListAsync();
        
        return result;
    }
}