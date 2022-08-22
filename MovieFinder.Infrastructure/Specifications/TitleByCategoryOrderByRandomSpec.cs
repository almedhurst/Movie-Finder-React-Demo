using Ardalis.Specification;
using Microsoft.EntityFrameworkCore;
using MovieFinder.Core.Entities;

namespace MovieFinder.Infrastructure.Specifications;

public class TitleByCategoryOrderByRandomSpec : Specification<Title>
{
    public TitleByCategoryOrderByRandomSpec(string categoryId, int amountToReturn)
    {
        Query.Include(x => x.TitleCategories)
            .ThenInclude(x => x.Category)
            .Include(x => x.TitleDirectors)
            .ThenInclude(x => x.Director)
            .Include(x => x.TitleActors)
            .ThenInclude(x => x.Actor)
            .Include(x => x.TitleWriters)
            .ThenInclude(x => x.Writer);

        Query.Where(x => x.TitleCategories.Any(x => x.Category.Id == categoryId))
            .OrderBy(_ => EF.Functions.Random()).Take(amountToReturn);
    }
}