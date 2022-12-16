using FamilyBudget.Common.Models.Input;

namespace FamilyBudget.Common.FilterPipelines;

public interface IPipelineFilter<T> where T : class
{
    public IQueryable<T> Execute(IQueryable<T> query, FilterInputModel input);
}