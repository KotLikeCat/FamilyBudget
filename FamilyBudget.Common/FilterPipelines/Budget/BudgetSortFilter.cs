using FamilyBudget.Common.Models.Input;
using Newtonsoft.Json;

namespace FamilyBudget.Common.FilterPipelines.Budget;

public class BudgetSortFilter : IPipelineFilter<Models.Data.Budget>
{
    public IQueryable<Models.Data.Budget> Execute(IQueryable<Models.Data.Budget> query, FilterInputModel input, params object[] dependencies)
    {
        if (input.Sort == null)
        {
            return query;
        }
        
        var list = JsonConvert.DeserializeObject<List<string>>(input.Sort);
        if (list is not { Count: 2 })
        {
            return query;
        }

        var result = query;
        result = list.First() switch
        {
            "createdAt" => list.Last() == "ASC"
                ? result.OrderBy(x => x.CreateTime)
                : result.OrderByDescending(x => x.CreateTime),
            _ => result
        };
        return result;
    }
}