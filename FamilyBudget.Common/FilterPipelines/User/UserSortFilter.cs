using FamilyBudget.Common.FilterPipelines;
using FamilyBudget.Common.Models.Input;
using Newtonsoft.Json;

namespace FamilyBudget.Common.FilterPipeLines.User;

public class UserSortFilter : IPipelineFilter<Models.Data.User>
{
    public IQueryable<Models.Data.User> Execute(IQueryable<Models.Data.User> query, FilterInputModel input, params object[] dependencies)
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
            "login" => list.Last() == "ASC" ?
                result.OrderBy(x => x.Login) 
                : result.OrderByDescending(x => x.Login),
            "createdAt" => list.Last() == "ASC"
                ? result.OrderBy(x => x.CreateTime)
                : result.OrderByDescending(x => x.CreateTime),
            "lastLoginAt" => list.Last() == "ASC"
                ? result.OrderBy(x => x.LastLoginTime)
                : result.OrderByDescending(x => x.LastLoginTime),
            _ => result
        };
        return result;
    }
}