namespace MovieFinder.Core.Entities;

public class Actor : BaseEntity
{
    public string Name { get; set; }
    public List<TitleActor> TitleActors { get; set; }
}