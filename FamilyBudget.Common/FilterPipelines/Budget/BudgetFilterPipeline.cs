namespace FamilyBudget.Common.FilterPipelines.Budget;

public class BudgetFilterPipeline : BaseFilterPipeline<Models.Data.Budget>
{
    public BudgetFilterPipeline(IQueryable<Models.Data.Budget> queryable) : base(queryable)
    {
        AddPipelineFilter(new BudgetSortFilter());
        AddPipelineFilter(new BudgetInputFilter());
    }
}