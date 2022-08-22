using Ardalis.Specification;
using Microsoft.EntityFrameworkCore;
using MovieFinder.Core.Entities;

namespace MovieFinder.Infrastructure.Specifications;

public class CategoryOrderByNameSpec : Specification<Category>
{
    public CategoryOrderByNameSpec()
    {
        Query.OrderBy(x => x.Name);
    }
}