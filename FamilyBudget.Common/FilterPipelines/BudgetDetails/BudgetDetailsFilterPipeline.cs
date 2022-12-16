namespace FamilyBudget.Common.FilterPipelines.BudgetDetails;

public class BudgetDetailsFilterPipeline : BaseFilterPipeline<Models.Data.BudgetDetail>
{
    public BudgetDetailsFilterPipeline(IQueryable<Models.Data.BudgetDetail> queryable) : base(queryable)
    {
        AddPipelineFilter(new BudgetDetailsSortFilter());
        AddPipelineFilter(new BudgetDetailsInputFilter());
    }
}