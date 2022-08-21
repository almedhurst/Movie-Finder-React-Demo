namespace MovieFinder.Core.Dtos;

public class PaginatedDto<T> where T : class
{
    public PaginatedDto(int pageIndex, int pageSize, int count, IEnumerable<T> data)
    {
        PaginationMeta = new PaginationMetaDto(pageIndex, pageSize, count);
        Data = data;
    }

    public PaginationMetaDto PaginationMeta { get; set; }
    public IEnumerable<T> Data { get; set; }
}