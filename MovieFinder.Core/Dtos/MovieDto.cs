namespace MovieFinder.Core.Dtos;

public class MovieDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public int Year { get; set; }
    public int RunTime { get; set; }
    public DateTime ReleaseDate { get; set; }
    public string? StoryLine { get; set; }
    public IEnumerable<CategoryDto> Categories { get; set; }
    public IEnumerable<NameDto> Directors { get; set; }
    public IEnumerable<NameDto> Writers { get; set; }
    public IEnumerable<NameDto> Actors { get; set; }
}