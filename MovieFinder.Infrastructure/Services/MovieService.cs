using Microsoft.EntityFrameworkCore;
using MovieFinder.Core.Dtos;
using MovieFinder.Core.Services;
using MovieFinder.Infrastructure.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieFinder.Core.Entities;
using MovieFinder.Core.RequestHelpers;
using MovieFinder.Infrastructure.Extensions;
using CategoryMovieDto = MovieFinder.Core.Dtos.CategoryMovieDto;

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

    public async Task<IEnumerable<CategoryMovieDto>> GetRandomMoviesByRandomCategories()
    {
        List<CategoryMovieDto> results = new List<CategoryMovieDto>();
        var data = await _context.Categories.OrderBy(_ => EF.Functions.Random())
            .Take(10)
            .Include(x => x.TitleCategories).ToListAsync();

        var cateogries = data.ToCategoryMovieDtoList();

        foreach (var item in cateogries)
        {
            var movieData = await _context.Titles.AddTitleIncludes()
                .Where(x => x.TitleCategories.Any(x => x.Category.Id == item.Id))
                .OrderBy(_ => EF.Functions.Random()).Take(8).ToListAsync();

            results.Add(new CategoryMovieDto
            {
                Id = item.Id,
                Name = item.Name,
                Movies = movieData.ToMovieDtoList()
            });
        }

        return results;
    }
}