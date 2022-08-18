namespace MovieFinder.Core.Entities;

public class Director : BaseEntity
{
    public string Name { get; set; }
    public List<TitleDirector> TitleDirectors { get; set; } = new();

}