using Ardalis.Specification;
using Microsoft.EntityFrameworkCore;
using MovieFinder.Core.Entities;

namespace MovieFinder.Infrastructure.Specifications;

public class CategoryOrderByRandomSpec : Specification<Category>
{
    public CategoryOrderByRandomSpec(int amountToReturn)
    {
        Query.OrderBy(_ => EF.Functions.Random()).Take(amountToReturn).Include(x => x.TitleCategories);
    }
}