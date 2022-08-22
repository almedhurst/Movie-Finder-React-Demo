namespace MovieFinder.Core.Dtos;

public class UserDto
{
    public string Email { get; set; }
    public string Token { get; set; }
    public string GivenName { get; set; }
    public List<FavouriteMovieDto> FavouriteMovies { get; set; } = new();
}