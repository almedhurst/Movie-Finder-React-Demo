namespace MovieFinder.Core.Entities;

public class Writer : BaseEntity
{
    public string Name { get; set; }
    public List<TitleWriter> TitleWriters { get; set; }
}