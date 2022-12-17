namespace FamilyBudget.Common.Models.View;
#pragma warning disable CS8618

public class BaseListViewModel<T> where T:class
{
    public List<T> Data { get; set; } = null!;
    public long Length { get; set; }
}