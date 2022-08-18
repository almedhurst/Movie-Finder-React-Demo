namespace MovieFinder.Core.Entities;

public class Category : BaseEntity
{
    public string Name { get; set; }
    public List<TitleCategory> TitleCategories { get; set; }
}