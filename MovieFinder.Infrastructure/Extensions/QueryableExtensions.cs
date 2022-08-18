using Microsoft.EntityFrameworkCore;
using MovieFinder.Core;
using MovieFinder.Core.Entities;
using MovieFinder.Core.RequestHelpers;

namespace MovieFinder.Infrastructure.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<Title> AddTitleFilters(this IQueryable<Title> query, MovieParams movieParams)
    {
        var categoryList = new List<string>();

        if (!string.IsNullOrWhiteSpace(movieParams.Categories))
            movieParams.Categories.Split(",").ToList();
        
        switch (movieParams.OrderBy)
        {
            case Enums.MovieOrderBy.DateAsc:
                query = query.OrderBy(x => x.ReleaseDate);
                break;
            case Enums.MovieOrderBy.DateDesc:
                query = query.OrderByDescending(x => x.ReleaseDate);
                break;
            case Enums.MovieOrderBy.Name:
                query = query.OrderBy(x => x.Name);
                break;
            case Enums.MovieOrderBy.Runtime:
                query = query.OrderBy(x => x.Runtime);
                break;
        }

        query = query.AddTitleIncludes()
            .Where(x => categoryList.Count == 0 || x.TitleCategories.Any(c => categoryList.Contains(c.CategoryId)))
            .Where(x => !movieParams.MinYear.HasValue || x.Year >= movieParams.MinYear.Value)
            .Where(x => !movieParams.MaxYear.HasValue || x.Year <= movieParams.MaxYear.Value);

        return query;
    }

    public static IQueryable<Title> AddTitleIncludes(this IQueryable<Title> query)
    {
        query = query.Include(x => x.TitleCategories)
            .ThenInclude(x => x.Category)
            .Include(x => x.TitleDirectors)
            .ThenInclude(x => x.Director)
            .Include(x => x.TitleActors)
            .ThenInclude(x => x.Actor)
            .Include(x => x.TitleWriters)
            .ThenInclude(x => x.Writer);
        return query;
    }
}