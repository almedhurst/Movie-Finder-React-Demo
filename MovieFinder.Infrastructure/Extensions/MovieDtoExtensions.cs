using MovieFinder.Core.Dtos;
using MovieFinder.Core.Entities;

namespace MovieFinder.Infrastructure.Extensions;

public static class MovieDtoExtensions
{
    public static IEnumerable<MovieDto> ToMovieDtoList(this IEnumerable<Title> data)
    {
        if (data == null) return Enumerable.Empty<MovieDto>();
        
        return data.Select(x => new MovieDto
            {
                Id = x.Id,
                Name = x.Name,
                Year = x.Year,
                RunTime = x.Runtime,
                ReleaseDate = x.ReleaseDate,
                StoryLine = x.StoryLine,
                Categories = x.TitleCategories.Select(c => new CategoryDto()
                {
                    Id = c.Category.Id,
                    Name = c.Category.Name
                }).OrderBy(c => c.Name),
                Directors = x.TitleDirectors.Select(d => new NameDto
                {
                    Id = d.Director.Id,
                    Name = d.Director.Name
                }).OrderBy(d => d.Name),
                Writers = x.TitleWriters.Select(w => new NameDto()
                {
                    Id = w.Writer.Id,
                    Name = w.Writer.Name
                }).OrderBy(w => w.Name),
                Actors = x.TitleActors.Select(a => new NameDto()
                {
                    Id = a.Actor.Id,
                    Name = a.Actor.Name
                }).OrderBy(a => a.Name)
            }
        );
    }

    public static MovieDto ToMovieDto(this Title data)
    {
        if (data == null) return null;
        return new MovieDto
        {
            Id = data.Id,
            Name = data.Name,
            Year = data.Year,
            RunTime = data.Runtime,
            ReleaseDate = data.ReleaseDate,
            StoryLine = data.StoryLine,
            Categories = data.TitleCategories?.Select(c => new CategoryDto()
            {
                Id = c.Category.Id,
                Name = c.Category.Name
            }).OrderBy(c => c.Name),
            Directors = data.TitleDirectors?.Select(d => new NameDto
            {
                Id = d.Director.Id,
                Name = d.Director.Name
            }).OrderBy(d => d.Name),
            Writers = data.TitleWriters?.Select(w => new NameDto()
            {
                Id = w.Writer.Id,
                Name = w.Writer.Name
            }).OrderBy(w => w.Name),
            Actors = data.TitleActors?.Select(a => new NameDto()
            {
                Id = a.Actor.Id,
                Name = a.Actor.Name
            }).OrderBy(a => a.Name)
        };
    }
}