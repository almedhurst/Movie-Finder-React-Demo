using Microsoft.EntityFrameworkCore;
using MovieFinder.Core.Dtos;
using MovieFinder.Core.Services;
using MovieFinder.Infrastructure.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieFinder.Core.RequestHelpers;
using MovieFinder.Infrastructure.Extensions;

namespace MovieFinder.Infrastructure.Services;

public class MovieService : IMovieService
{
    private readonly MovieContext _context;

    public MovieService(MovieContext context)
    {
        _context = context;
    }
    
    public async Task<PaginatedDto<MovieDto>> GetMovies(MovieParams movieParams, int pageIndex = 1, int pageSize = 50)
    {
        var query = _context.Titles.AddTitleFilters(movieParams);

        var count = await query.CountAsync();
        var data = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

        return new PaginatedDto<MovieDto>(pageIndex, pageSize, count, data.ToMovieDtoList());
    }

    public async Task<MovieDto> GetMovie(string movieId)
    {
        var query = _context.Titles.Where(x => x.Id == movieId)
            .AddTitleIncludes();

        var data = await query.FirstOrDefaultAsync();

        return data.ToMovieDto();
    }
}