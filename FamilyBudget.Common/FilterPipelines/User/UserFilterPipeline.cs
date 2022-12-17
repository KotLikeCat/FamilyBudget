namespace FamilyBudget.Common.FilterPipelines.User;

public class UserFilterPipeline : BaseFilterPipeline<Models.Data.User>
{
    public UserFilterPipeline(IQueryable<Models.Data.User> queryable) : base(queryable)
    {
        AddPipelineFilter(new UserSortFilter());
        AddPipelineFilter(new UserInputFilter());
    }
}