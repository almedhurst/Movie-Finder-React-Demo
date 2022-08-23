using Microsoft.AspNetCore.Mvc;
using MovieFinder.Core.Dtos;
using MovieFinder.Core.RequestHelpers;
using MovieFinder.Core.Services;
using MovieFinder.Web.Extensions;

namespace MovieFinder.Web.Controllers;

public class MoviesController : BaseApiController
{
    private readonly IMovieService _movieService;

    public MoviesController(IMovieService movieService)
    {
        _movieService = movieService;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MovieDto>>> GetMovies([FromQuery] MovieParams movieParams, int pageNumber = 1, int pageSize = 50)
    {
        if(pageNumber < 1) pageNumber = 1;
        if (pageSize > 50) pageSize = 50;
        if (pageSize < 3) pageSize = 3;
        var data = await _movieService.GetMovies(movieParams,pageNumber, pageSize);
        Response.AddPaginationHeader(data.PaginationMeta);
        return Ok(data.Data);
    }

    [HttpGet]
    public async Task<ActionResult<MovieDto>> GetMovie(string movieId)
    {
        var data = await _movieService.GetMovie(movieId);
        if (data == null) return NotFound();

        return Ok(data);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryMovieDto>>> GetRandomMoviesByRandomCategories()
    {
        var data = await _movieService.GetRandomMoviesByRandomCategories();
        return Ok(data);
    }
}