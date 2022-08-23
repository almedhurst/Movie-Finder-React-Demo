using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MovieFinder.Core.Dtos;
using MovieFinder.Core.Entities;
using MovieFinder.Core.Repositories;
using MovieFinder.Core.Services;
using MovieFinder.Infrastructure.Data;
using MovieFinder.Infrastructure.Extensions;
using MovieFinder.Infrastructure.Specifications;

namespace MovieFinder.Infrastructure.Services;

public class UserService : IUserService
{
    private readonly UserManager<User> _userManager;
    private ITokenService _tokenService;
    private readonly IGenericRepository<UserFavouriteTitle> _favouriteTitleRepo;
    private readonly IGenericRepository<Title> _titleRepo;

    public UserService(UserManager<User> userManager,
        ITokenService tokenService, IGenericRepository<UserFavouriteTitle> favouriteTitleRepo,
        IGenericRepository<Title> titleRepo)
    {
        _titleRepo = titleRepo;
        _favouriteTitleRepo = favouriteTitleRepo;
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
        var existingFavSpec = new UserFavouriteTitleByUserIdTitleIdSpec(user.Id, model.TitleId);
        var existingFavourite = await _favouriteTitleRepo.FirstOrDefaultAsync(existingFavSpec);

        if (existingFavourite != null)
        {
            existingFavourite.Comments = model.Comments;
            await _favouriteTitleRepo.UpdateAsync(existingFavourite);
        }
        else
        {
            await _favouriteTitleRepo.AddAsync(new UserFavouriteTitle
            {
                UserId = user.Id,
                TitleId = model.TitleId,
                Comments = model.Comments
            });
        }
        
        var userData = await GetUserDto(user);
        return userData.FavouriteMovies;
    }

    public async Task<List<FavouriteMovieDto>> RemoveFavouriteMovie(string username, AddRemoveFavouriteMovieDto model)
    {
        var user = await _userManager.FindByNameAsync(username);
        var favMovieSpec = new UserFavouriteTitleByUserIdTitleIdSpec(user.Id, model.TitleId);
        var favMovie = await _favouriteTitleRepo.FirstOrDefaultAsync(favMovieSpec);

        if (favMovie != null)
        {
            await _favouriteTitleRepo.DeleteAsync(favMovie);
        }
        
        var userData = await GetUserDto(user);
        return userData.FavouriteMovies;
    }

    public async Task<IdentityResult> RegisterUser(RegisterDto model)
    {
        var user = new User
        {
            GivenName = model.GivenName,
            UserName = model.Username,
            Email = model.Email
        };

        return await _userManager.CreateAsync(user, model.Password);
    }
    
    public virtual async Task<UserDto> GetUserDto(User user)
    {
        var favMoviesSpec = new UserFavouriteTitlesByUserIdSpec(user.Id);
        var favMovies = await _favouriteTitleRepo.ListAsync(favMoviesSpec);
        var movies = new List<Title>();

        if (favMovies.Any())
        {
            var movieSpec = new TitlesByUserFavouriteTitleSpec(favMovies.Select(x => x.TitleId));
            movies = await _titleRepo.ListAsync(movieSpec);
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