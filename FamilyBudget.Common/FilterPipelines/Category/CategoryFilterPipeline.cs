namespace FamilyBudget.Common.FilterPipelines.Category;

public class CategoryFilterPipeline : BaseFilterPipeline<Models.Data.Category>
{
    public CategoryFilterPipeline(IQueryable<Models.Data.Category> queryable) : base(queryable)
    {
        AddPipelineFilter(new CategorySortFilter());
    }
}