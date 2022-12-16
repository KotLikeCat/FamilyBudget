using FamilyBudget.Common.FilterPipelines;
using FamilyBudget.Common.Models.Input;
using Newtonsoft.Json;

namespace FamilyBudget.Common.FilterPipeLines.User;

public class UserInputFilter : IPipelineFilter<Models.Data.User>
{
    public IQueryable<Models.Data.User> Execute(IQueryable<Models.Data.User> query, FilterInputModel input)
    {
        var filters = JsonConvert.DeserializeObject<Dictionary<string, string>>(input.Filter);
        if (filters == null || !filters.Any())
        {
            return query;
        }

        var result = query;
        if (filters.TryGetValue("user", out var userFilter))
        {
            result = result.Where(x => x.Login.Contains(userFilter));
        }

        return result;
    }
}