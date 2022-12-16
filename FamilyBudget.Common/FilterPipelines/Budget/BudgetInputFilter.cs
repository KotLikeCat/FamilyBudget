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
        if (filters.TryGetValue("name", out var nameFilter))
        {
            result = result.Where(x => x.Name.Contains(nameFilter));
        }
        
        if (filters.TryGetValue("description", out var descriptionFilter))
        {
            result = result.Where(x => x.Description != null && x.Description.Contains(descriptionFilter));
        }
        
        if (filters.TryGetValue("owner", out var ownerFilter))
        {
            result = result.Where(x => x.User.Login.Contains(ownerFilter));
        }

        return result;
    }
}