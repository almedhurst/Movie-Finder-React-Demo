using Microsoft.EntityFrameworkCore;
using MovieFinder.Core.Dtos;
using MovieFinder.Core.Services;
using MovieFinder.Infrastructure.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieFinder.Core.Entities;
using MovieFinder.Core.Repositories;
using MovieFinder.Core.RequestHelpers;
using MovieFinder.Infrastructure.Extensions;
using MovieFinder.Infrastructure.Specifications;
using CategoryMovieDto = MovieFinder.Core.Dtos.CategoryMovieDto;

namespace MovieFinder.Infrastructure.Services;

public class MovieService : IMovieService
{
    private readonly IGenericRepository<Title> _titleRepo;
    private readonly IGenericRepository<Category> _categoryRepo;

    public MovieService(IGenericRepository<Title> titleRepo, IGenericRepository<Category> categoryRepo)
    {
        _titleRepo = titleRepo;
        _categoryRepo = categoryRepo;
    }
    
    public async Task<PaginatedDto<MovieDto>> GetMovies(MovieParams movieParams, int pageIndex = 1, int pageSize = 50)
    {
        var countSpec = new TitlesSpec(movieParams);
        var dataSpec = new TitlesSpec(movieParams, pageIndex, pageSize);
        var data = await _titleRepo.ListAsync(dataSpec);
        var count = await _titleRepo.CountAsync(countSpec);

        return new PaginatedDto<MovieDto>(pageIndex, pageSize, count, data.ToMovieDtoList());
    }

    public async Task<MovieDto> GetMovie(string movieId)
    {
        var dataSpec = new TitleByIdSpec(movieId);
        var data = await _titleRepo.FirstOrDefaultAsync(dataSpec);
        
        return data.ToMovieDto();
    }

    public async Task<IEnumerable<CategoryMovieDto>> GetRandomMoviesByRandomCategories()
    {
        List<CategoryMovieDto> results = new List<CategoryMovieDto>();

        var dataSpec = new CategoryOrderByRandomSpec(10);
        var data = await _categoryRepo.ListAsync(dataSpec);
        
        var cateogries = data.ToCategoryMovieDtoList();

        foreach (var item in cateogries)
        {
            var movieDataSpec = new TitleByCategoryOrderByRandomSpec(item.Id, 8);
            var movieData = await _titleRepo.ListAsync(movieDataSpec);

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