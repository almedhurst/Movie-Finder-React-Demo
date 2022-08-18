namespace MovieFinder.Core.Dtos;

public class PaginatedDto<T> where T : class
{
    public PaginatedDto(int pageIndex, int pageSize, int count, IEnumerable<T> data)
    {
        PageIndex = pageIndex;
        PageSize = pageSize;
        Count = count;
        Data = data;
    }

    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public int Count { get; set; }
    public int PageCount
    {
        get { return (Count + PageSize - 1) / PageSize; }
    }
    public IEnumerable<T> Data { get; set; }
}