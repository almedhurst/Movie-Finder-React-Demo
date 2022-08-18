using Microsoft.EntityFrameworkCore;
using MovieFinder.Core.Dtos;
using MovieFinder.Core.Services;
using MovieFinder.Infrastructure.Data;
using MovieFinder.Infrastructure.Extensions;

namespace MovieFinder.Infrastructure.Services;

public class CategoryService : ICategoryService
{
    private readonly MovieContext _context;

    public CategoryService(MovieContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<NameDto>> GetCategories()
    {
        var query = _context.Categories.OrderBy(x => x.Name);

        var data = await query.ToListAsync();

        return data.ToNameDtoList();
    }
}