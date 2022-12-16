using FamilyBudget.Common.FilterPipelines;
using FamilyBudget.Common.Models.Input;
using Newtonsoft.Json;

namespace FamilyBudget.Common.FilterPipeLines.Generic;

public class Pagination<T> : IPipelineFilter<T> where T:class
{
    public IQueryable<T> Execute(IQueryable<T> query, FilterInputModel input)
    {
        var rangeArray = JsonConvert.DeserializeObject<List<int>>(input.Range);
        if (rangeArray is not { Count: 2 })
        {
            return query;
        }

        var difference = rangeArray.Last() - rangeArray.First() + 1;
        return query.Skip(rangeArray.First()).Take(difference);
    }
}