namespace MovieFinder.Core.RequestHelpers;

public class MovieParams
{
    public Enums.MovieOrderBy OrderBy { get; set; } = Enums.MovieOrderBy.DateDesc;
    public string? Categories { get; set; }
    public int? MinYear { get; set; }
    public int? MaxYear { get; set; }
}