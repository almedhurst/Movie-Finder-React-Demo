using System.Collections;
using MovieFinder.Core.Dtos;
using MovieFinder.Core.Entities;

namespace MovieFinder.Infrastructure.Extensions;

public static class CategoryMovieDtoExtensions
{
    public static IEnumerable<CategoryMovieDto> ToCategoryMovieDtoList(this IEnumerable<Category> data)
    {
        if (data == null) return Enumerable.Empty<CategoryMovieDto>();
        return data.Select(x => new Core.Dtos.CategoryMovieDto
        {
            Id = x.Id,
            Name = x.Name,
            Movies = new List<MovieDto>()
        });
    }
}