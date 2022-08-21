using MovieFinder.Core.Dtos;
using MovieFinder.Core.Entities;

namespace MovieFinder.Infrastructure.Extensions;

public static class CategoryMovieDto
{
    public static IEnumerable<Core.Dtos.CategoryMovieDto> ToCategoryMovieDtoList(this IEnumerable<Category> data)
    {
        return data.Select(x => new Core.Dtos.CategoryMovieDto
        {
            Id = x.Id,
            Name = x.Name,
            Movies = new List<MovieDto>()
        });
    }
}