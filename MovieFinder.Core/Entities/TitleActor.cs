namespace MovieFinder.Core.Entities;

public class TitleActor : BaseEntity
{
    public string TitleId { get; set; }
    public Title Title { get; set; }
    public string ActorId { get; set; }
    public Actor Actor { get; set; }
}