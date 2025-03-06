namespace Domain.Common;

public class ResultPage<T>
{
    public int PageSize { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int CountItems { get; set; }
    public IEnumerable<T> Items { get; set; }

    public bool HasPrevious => CurrentPage != 1;
    public bool HasNext => CurrentPage < TotalPages;

    public ResultPage(int pageSize, int currentPage, int count, IEnumerable<T> items)
    {
        PageSize = pageSize;
        CurrentPage = currentPage;
        TotalPages = (int)Math.Ceiling(CountItems / (float)PageSize);
        CountItems = count;
        Items = items;
    }
}
