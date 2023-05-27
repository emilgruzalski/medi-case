namespace MediCaseAPI.DataTransferObjects;

public class PagedResultDTO<T>
{
    public PagedResultDTO(IEnumerable<T> items, int totalCount, PagingParameters parameters)
    {
        Items = items.ToList();
        TotalCount = totalCount;
        Page = parameters.Page;
        PageSize = parameters.PageSize;
    }

    public List<T> Items { get; }
    public int Page { get; }
    public int PageSize { get; }
    public int TotalCount { get; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
}
