namespace MovieFinder.Core.Entities;

public class UserFavouriteTitle : BaseEntity
{
    public string UserId { get; set; }
    public User User { get; set; }
    public string TitleId { get; set; }
    public Title Title { get; set; }
    public string? Comments { get; set; }
}