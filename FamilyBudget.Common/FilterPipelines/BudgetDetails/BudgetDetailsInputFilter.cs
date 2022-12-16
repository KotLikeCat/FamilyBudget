using FamilyBudget.Common.Models.Input;
using Newtonsoft.Json;

namespace FamilyBudget.Common.FilterPipelines.BudgetDetails;

public class BudgetDetailsInputFilter : IPipelineFilter<Models.Data.BudgetDetail>
{
    public IQueryable<Models.Data.BudgetDetail> Execute(IQueryable<Models.Data.BudgetDetail> query, FilterInputModel input, params object[] dependencies)
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
        if (filters.TryGetValue("category", out var categoryFilter))
        {
            result = result.Where(x => x.Category.Name.Contains(categoryFilter));
        }
        
        if (filters.TryGetValue("description", out var descriptionFilter))
        {
            result = result.Where(x => x.Description != null && x.Description.Contains(descriptionFilter));
        }
        
        if (filters.TryGetValue("owner", out var ownerFilter))
        {
            result = result.Where(x => x.User.Login.Contains(ownerFilter));
        }
        
        if (filters.TryGetValue("type", out var typeFilter))
        {
            var isIncome = typeFilter == "income";
            result = result.Where(x => x.IsIncome == isIncome);
        }

        return result;
    }
}