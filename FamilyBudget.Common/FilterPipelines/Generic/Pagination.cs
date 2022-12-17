using FamilyBudget.Common.Models.Input;
using Newtonsoft.Json;

namespace FamilyBudget.Common.FilterPipelines.Generic;

public class Pagination<T> : IPipelineFilter<T> where T:class
{
    public IQueryable<T> Execute(IQueryable<T> query, FilterInputModel input, params object[] dependencies)
    {
        if (input.Range == null)
        {
            return query;
        }
        
        var rangeArray = JsonConvert.DeserializeObject<List<int>>(input.Range);
        if (rangeArray is not { Count: 2 })
        {
            return query;
        }

        var difference = rangeArray.Last() - rangeArray.First() + 1;
        
        return query.Skip(rangeArray.First()).Take(difference);
    }
}