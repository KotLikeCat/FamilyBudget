namespace FamilyBudget.Common.Models.View;

public class BaseListViewModel<T> where T:class
{
    public BaseListViewModel()
    {
        
    }
    public BaseListViewModel(List<T> data, long length)
    {
        Data = data;
        Length = length;
    }

    public List<T> Data { get; set; } = null!;
    public long Length { get; set; }
}