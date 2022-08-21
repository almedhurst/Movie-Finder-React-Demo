namespace MovieFinder.Core.Dtos;

public class CategoryMovieDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public IEnumerable<MovieDto> Movies { get; set; }
}