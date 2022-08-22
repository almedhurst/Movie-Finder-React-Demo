using Ardalis.Specification;
using MovieFinder.Core.Entities;

namespace MovieFinder.Infrastructure.Specifications;

public class TitlesByUserFavouriteTitleSpec : Specification<Title>
{
    public TitlesByUserFavouriteTitleSpec(IEnumerable<string> titleIds)
    {
        Query.Where(x => titleIds.Contains(x.Id))
            .Include(x => x.TitleCategories)
            .ThenInclude(x => x.Category)
            .Include(x => x.TitleDirectors)
            .ThenInclude(x => x.Director)
            .Include(x => x.TitleActors)
            .ThenInclude(x => x.Actor)
            .Include(x => x.TitleWriters)
            .ThenInclude(x => x.Writer);
    }
}