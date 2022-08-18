namespace MovieFinder.Core.Entities;

public class TitleDirector : BaseEntity
{
    public string TitleId { get; set; }
    public Title Title { get; set; }
    public string DirectorId { get; set; }
    public Director Director { get; set; }
}