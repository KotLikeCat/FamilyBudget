using FamilyBudget.Common.Models.Input;
using Newtonsoft.Json;

namespace FamilyBudget.Common.FilterPipelines.Category;

public class CategorySortFilter : IPipelineFilter<Models.Data.Category>
{
    public IQueryable<Models.Data.Category> Execute(IQueryable<Models.Data.Category> query, FilterInputModel input, params object[] dependencies)
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
            "name" => list.Last() == "ASC"
                ? result.OrderBy(x => x.Name)
                : result.OrderByDescending(x => x.Name),
            _ => result
        };
        return result;
    }
}