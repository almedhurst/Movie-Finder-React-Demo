using MovieFinder.Core.Dtos;
using MovieFinder.Core.Entities;

namespace MovieFinder.Infrastructure.Extensions;

public static class CategoryDtoExtensions
{
    public static IEnumerable<NameDto> ToNameDtoList(this IEnumerable<Category> data)
    {
        return data.Select(x => new NameDto {Id = x.Id, Name = x.Name});
    }
}