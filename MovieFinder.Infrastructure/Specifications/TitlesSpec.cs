using Ardalis.Specification;
using MovieFinder.Core;
using MovieFinder.Core.Entities;
using MovieFinder.Core.RequestHelpers;

namespace MovieFinder.Infrastructure.Specifications;

public class TitlesSpec : Specification<Title>
{
    public TitlesSpec(MovieParams movieParams, int pageIndex, int pageSize)
    {
        var categoryList = new List<string>();

        if (!string.IsNullOrWhiteSpace(movieParams.Categories))
            categoryList = movieParams.Categories.Split(",").ToList();
        
        switch (movieParams.OrderBy)
        {
            case Enums.MovieOrderBy.DateAsc:
                Query.OrderBy(x => x.ReleaseDate);
                break;
            case Enums.MovieOrderBy.DateDesc:
                Query.OrderByDescending(x => x.ReleaseDate);
                break;
            case Enums.MovieOrderBy.Name:
                Query.OrderBy(x => x.Name);
                break;
            case Enums.MovieOrderBy.Runtime:
                Query.OrderBy(x => x.Runtime);
                break;
        }

        Query.Include(x => x.TitleCategories)
            .ThenInclude(x => x.Category)
            .Include(x => x.TitleDirectors)
            .ThenInclude(x => x.Director)
            .Include(x => x.TitleActors)
            .ThenInclude(x => x.Actor)
            .Include(x => x.TitleWriters)
            .ThenInclude(x => x.Writer);
        
        Query.Where(x => categoryList.Count == 0 || x.TitleCategories.Any(c => categoryList.Contains(c.CategoryId)))
            .Where(x => !movieParams.MinYear.HasValue || x.Year >= movieParams.MinYear.Value)
            .Where(x => !movieParams.MaxYear.HasValue || x.Year <= movieParams.MaxYear.Value);
        
        Query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
    }
    
    public TitlesSpec(MovieParams movieParams)
    {
        var categoryList = new List<string>();

        if (!string.IsNullOrWhiteSpace(movieParams.Categories))
            categoryList = movieParams.Categories.Split(",").ToList();
        
        switch (movieParams.OrderBy)
        {
            case Enums.MovieOrderBy.DateAsc:
                Query.OrderBy(x => x.ReleaseDate);
                break;
            case Enums.MovieOrderBy.DateDesc:
                Query.OrderByDescending(x => x.ReleaseDate);
                break;
            case Enums.MovieOrderBy.Name:
                Query.OrderBy(x => x.Name);
                break;
            case Enums.MovieOrderBy.Runtime:
                Query.OrderBy(x => x.Runtime);
                break;
        }

        Query.Include(x => x.TitleCategories)
            .ThenInclude(x => x.Category)
            .Include(x => x.TitleDirectors)
            .ThenInclude(x => x.Director)
            .Include(x => x.TitleActors)
            .ThenInclude(x => x.Actor)
            .Include(x => x.TitleWriters)
            .ThenInclude(x => x.Writer);
        
        Query.Where(x => categoryList.Count == 0 || x.TitleCategories.Any(c => categoryList.Contains(c.CategoryId)))
            .Where(x => !movieParams.MinYear.HasValue || x.Year >= movieParams.MinYear.Value)
            .Where(x => !movieParams.MaxYear.HasValue || x.Year <= movieParams.MaxYear.Value);
        
    }
}