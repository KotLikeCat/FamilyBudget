using FamilyBudget.Common.Models.Input;
using Newtonsoft.Json;

namespace FamilyBudget.Common.FilterPipelines.BudgetDetails;

public class BudgetDetailsSortFilter : IPipelineFilter<Models.Data.BudgetDetail>
{
    public IQueryable<Models.Data.BudgetDetail> Execute(IQueryable<Models.Data.BudgetDetail> query, FilterInputModel input, params object[] dependencies)
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
            "amount" => list.Last() == "ASC"
                ? result.OrderBy(x => x.Amount)
                : result.OrderByDescending(x => x.Amount),
            "isIncome" => list.Last() == "ASC"
                ? result.OrderBy(x => x.IsIncome)
                : result.OrderByDescending(x => x.IsIncome),
            "category" => list.Last() == "ASC"
                ? result.OrderBy(x => x.Category.Name)
                : result.OrderByDescending(x => x.Category.Name),
            "description" => list.Last() == "ASC"
                ? result.OrderBy(x => x.Description)
                : result.OrderByDescending(x => x.Description),
            "owner" => list.Last() == "ASC"
                ? result.OrderBy(x => x.User.Login)
                : result.OrderByDescending(x => x.User.Login),
            _ => result
        };
        
        return result;
    }
}