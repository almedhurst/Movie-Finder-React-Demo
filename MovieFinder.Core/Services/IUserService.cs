using Microsoft.AspNetCore.Identity;
using MovieFinder.Core.Dtos;

namespace MovieFinder.Core.Services;

public interface IUserService
{
    Task<UserDto?> Login(LoginDto model);
    Task<UserDto?> GetCurrentUser(string username);
    Task<List<FavouriteMovieDto>> GetUserFavouriteMovies(string username);

    Task<List<FavouriteMovieDto>> AddFavouriteMovie(string username, AddRemoveFavouriteMovieDto model);
    Task<List<FavouriteMovieDto>> RemoveFavouriteMovie(string username, AddRemoveFavouriteMovieDto model);

    Task<IdentityResult> RegisterUser(RegisterDto model);
}