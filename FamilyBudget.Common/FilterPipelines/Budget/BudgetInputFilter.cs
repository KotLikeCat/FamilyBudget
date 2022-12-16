using FamilyBudget.Common.Models.Input;
using Newtonsoft.Json;

namespace FamilyBudget.Common.FilterPipelines.Budget;

public class BudgetInputFilter : IPipelineFilter<Models.Data.Budget>
{
    public IQueryable<Models.Data.Budget> Execute(IQueryable<Models.Data.Budget> query, FilterInputModel input, params object[] dependencies)
    {
        if (input.Filter == null)
        {
            return query;
        }
        
        var filters = JsonConvert.DeserializeObject<Dictionary<string, string>>(input.Filter);
        if (filters == null || !filters.Any())
        {
            return query;
        }

        var result = query;
        if (filters.TryGetValue("user", out var userFilter))
        {
            //
        }

        return result;
    }
}