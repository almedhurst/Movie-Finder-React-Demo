namespace MovieFinder.Core.Entities;

public class TitleWriter : BaseEntity
{
    public string TitleId { get; set; }
    public Title Title { get; set; }
    public string WriterId { get; set; }
    public Writer Writer { get; set; }
}