using FamilyBudget.Common.FilterPipelines;
using FamilyBudget.Common.Models.Input;
using Newtonsoft.Json;

namespace FamilyBudget.Common.FilterPipeLines.User;

public class UserInputFilter : IPipelineFilter<Models.Data.User>
{
    public IQueryable<Models.Data.User> Execute(IQueryable<Models.Data.User> query, FilterInputModel input, params object[] dependencies)
    {
        if (input.Filter == null)
        {
            return query;
        }
        
        var filters = JsonConvert.DeserializeObject<Dictionary<string, object>>(input.Filter);
        if (filters == null || !filters.Any())
        {
            return query;
        }

        var result = query;
        if (filters.TryGetValue("user", out var userFilter))
        {
            result = result.Where(x => x.Login.Contains((userFilter as string)!));
        }

        if (filters.TryGetValue("id", out var idFilter))
        {
            var arr = JsonConvert.DeserializeObject<List<Guid>>(idFilter.ToString()!);
            result = result.Where(x => arr!.Any(y => x.Id == y));
        }

        if (filters.TryGetValue("hideCaller", out var hideCallerFilter))
        {
            var value = hideCallerFilter as bool? ?? false;
            var user = dependencies.SingleOrDefault(x => x.GetType() == typeof(Models.Data.User));

            if (user != null)
            {
                var id = (user as Models.Data.User)!.Id;
                result = result.Where(x => x.Id != id);
            }
        }

        return result;
    }
}