namespace MovieFinder.Core.Entities;

public class Title : BaseEntity
{
    public string Name { get; set; }
    public int Year { get; set; }
    public int Runtime { get; set; }
    public List<TitleCategory> TitleCategories { get; set; }
    public DateTime ReleaseDate { get; set; }
    public List<TitleDirector> TitleDirectors { get; set; }
    public List<TitleWriter> TitleWriters { get; set; }
    public List<TitleActor> TitleActors { get; set; }
    public string? StoryLine { get; set; }
}