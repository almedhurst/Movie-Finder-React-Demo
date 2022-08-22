using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieFinder.Core.Dtos;
using MovieFinder.Core.Services;

namespace MovieFinder.Web.Controllers;

public class AccountController : BaseApiController
{
    private readonly IUserService _userService;

    public AccountController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public async Task<ActionResult<UserDto>> Login(LoginDto model)
    {
        var userData = await _userService.Login(model);

        if (userData == null) return Unauthorized();

        return Ok(userData);
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<UserDto>> GetCurrentUser()
    {
        var userData = await _userService.GetCurrentUser(User.Identity.Name);
        return Ok(userData);
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<List<FavouriteMovieDto>>> GetFavouriteMovies()
    {
        var data = await _userService.GetUserFavouriteMovies(User.Identity.Name);
        return Ok(data);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<List<FavouriteMovieDto>>> AddFavouriteMovie(AddRemoveFavouriteMovieDto model)
    {
        var data = await _userService.AddFavouriteMovie(User.Identity.Name, model);
        return Ok(data);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<List<FavouriteMovieDto>>> DeleteFavouriteMovie(AddRemoveFavouriteMovieDto model)
    {
        var data = await _userService.RemoveFavouriteMovie(User.Identity.Name, model);
        return Ok(data);
    }

    [HttpPost]
    public async Task<ActionResult> Register(RegisterDto model)
    {
        var result = await _userService.RegisterUser(model);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }

            return ValidationProblem();
        }

        return StatusCode(201);
    }
    
}