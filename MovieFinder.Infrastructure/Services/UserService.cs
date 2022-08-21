using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MovieFinder.Core.Dtos;
using MovieFinder.Core.Entities;
using MovieFinder.Core.Services;
using MovieFinder.Infrastructure.Data;
using MovieFinder.Infrastructure.Extensions;

namespace MovieFinder.Infrastructure.Services;

public class UserService : IUserService
{
    private readonly UserManager<User> _userManager;
    private ITokenService _tokenService;
    private MovieContext _movieContext;

    public UserService(UserManager<User> userManager,
        ITokenService tokenService, MovieContext movieContext)
    {
        _movieContext = movieContext;
        _tokenService = tokenService;
        _userManager = userManager;
    }
    
    public async Task<UserDto?> Login(LoginDto model)
    {
        var user = await _userManager.FindByNameAsync(model.Username);
        if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            return null;

        return await GetUserDto(user);
    }

    public async Task<UserDto?> GetCurrentUser(string username)
    {
        var user = await _userManager.FindByNameAsync(username);
        return await GetUserDto(user);
    }

    public async Task<List<FavouriteMovieDto>> GetUserFavouriteMovies(string username)
    {
        var user = await _userManager.FindByNameAsync(username);
        var userData = await GetUserDto(user);
        return userData.FavouriteMovies;
    }

    public async Task<List<FavouriteMovieDto>> AddFavouriteMovie(string username, AddRemoveFavouriteMovieDto model)
    {
        var user = await _userManager.FindByNameAsync(username);
        await _movieContext.UserFavouriteTitles.AddAsync(new UserFavouriteTitle
        {
            UserId = user.Id,
            TitleId = model.TitleId,
            Comments = model.Comments
        });
        
        var userData = await GetUserDto(user);
        return userData.FavouriteMovies;
    }

    public async Task<List<FavouriteMovieDto>> RemoveFavouriteMovie(string username, AddRemoveFavouriteMovieDto model)
    {
        var user = await _userManager.FindByNameAsync(username);
        var favMovie = await _movieContext.UserFavouriteTitles
            .FirstOrDefaultAsync(x => x.UserId == user.Id && x.TitleId == model.TitleId);

        if (favMovie != null) _movieContext.UserFavouriteTitles.Remove(favMovie);
        
        var userData = await GetUserDto(user);
        return userData.FavouriteMovies;
    }
    
    private async Task<UserDto> GetUserDto(User user)
    {
        var favMovies = await _movieContext.UserFavouriteTitles.Where(x => x.UserId == user.Id).ToListAsync();
        var movies = new List<Title>();

        if (favMovies.Any())
        {
            movies = await _movieContext.Titles.Where(x => favMovies.Any(y => y.TitleId == x.Id)).AddTitleIncludes()
                .ToListAsync();
        }
        
        return new UserDto()
        {
            Email = user.Email,
            GivenName = user.GivenName,
            Token = _tokenService.GenerateToken(user),
            FavouriteMovies = favMovies.Select(x => new FavouriteMovieDto
            {
                Comments = x.Comments,
                Movie = movies.FirstOrDefault(y => x.TitleId == y.Id).ToMovieDto()
            }).ToList()
        };
    }
}