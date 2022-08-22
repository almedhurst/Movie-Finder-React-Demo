using Ardalis.Specification;
using MovieFinder.Core.Entities;

namespace MovieFinder.Infrastructure.Specifications;

public class TitleByIdSpec : Specification<Title>, ISingleResultSpecification
{
    public TitleByIdSpec(string id)
    {
        Query.Where(x => x.Id == id);
        
        Query.Include(x => x.TitleCategories)
            .ThenInclude(x => x.Category)
            .Include(x => x.TitleDirectors)
            .ThenInclude(x => x.Director)
            .Include(x => x.TitleActors)
            .ThenInclude(x => x.Actor)
            .Include(x => x.TitleWriters)
            .ThenInclude(x => x.Writer);
    }
}