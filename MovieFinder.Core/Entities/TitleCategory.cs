namespace MovieFinder.Core.Entities;

public class TitleCategory : BaseEntity
{
    public string TitleId { get; set; }
    public Title Title { get; set; }
    public string CategoryId { get; set; }
    public Category Category { get; set; }
}