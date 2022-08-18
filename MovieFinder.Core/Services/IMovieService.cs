using MovieFinder.Core.Dtos;
using MovieFinder.Core.RequestHelpers;

namespace MovieFinder.Core.Services;

public interface IMovieService
{
    Task<PaginatedDto<MovieDto>> GetMovies(MovieParams movieParams, int pageIndex = 1, int pageSize = 50);

    Task<MovieDto> GetMovie(string movieId);
}