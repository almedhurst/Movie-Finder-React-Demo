namespace MovieFinder.Core.Dtos;

public class PaginationMetaDto
{
    public PaginationMetaDto(int pageIndex, int pageSize, int count)
    {
        PageIndex = pageIndex;
        PageSize = pageSize;
        Count = count;
    }
    public int PageIndex { get; }
    public int PageSize { get; }
    public int Count { get; }
    public int PageCount
    {
        get { return (Count + PageSize - 1) / PageSize; }
    }
}