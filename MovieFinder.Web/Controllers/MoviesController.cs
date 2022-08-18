using Microsoft.AspNetCore.Mvc;
using MovieFinder.Core.Dtos;
using MovieFinder.Core.RequestHelpers;
using MovieFinder.Core.Services;

namespace MovieFinder.Web.Controllers;

public class MoviesController : BaseApiController
{
    private readonly IMovieService _movieService;

    public MoviesController(IMovieService movieService)
    {
        _movieService = movieService;
    }
    
    [HttpGet]
    public async Task<ActionResult<PaginatedDto<MovieDto>>> GetMovies([FromQuery] MovieParams movieParams, int pageNumber = 1, int pageSize = 50)
    {
        if(pageNumber < 1) pageNumber = 1;
        if (pageSize > 50) pageSize = 50;
        if (pageSize < 10) pageSize = 10;
        return await _movieService.GetMovies(movieParams,pageNumber, pageSize);
    }

    [HttpGet]
    public async Task<ActionResult<MovieDto>> GetMovie(string movieId)
    {
        var data = await _movieService.GetMovie(movieId);
        if (data == null) return NotFound();

        return data;
    }
}