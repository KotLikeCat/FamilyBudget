namespace FamilyBudget.Common.Models.Data;

public abstract class BaseDataModel
{
    protected BaseDataModel()
    {
        Id = Guid.NewGuid();
    }

    public long Index { get; set; }
    public Guid Id { get; set; }
}