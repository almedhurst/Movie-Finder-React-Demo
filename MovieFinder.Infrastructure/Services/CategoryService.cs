using Microsoft.EntityFrameworkCore;
using MovieFinder.Core.Dtos;
using MovieFinder.Core.Entities;
using MovieFinder.Core.Repositories;
using MovieFinder.Core.Services;
using MovieFinder.Infrastructure.Data;
using MovieFinder.Infrastructure.Extensions;
using MovieFinder.Infrastructure.Specifications;

namespace MovieFinder.Infrastructure.Services;

public class CategoryService : ICategoryService
{
    private readonly IGenericRepository<Category> _categoryRepo;

    public CategoryService(IGenericRepository<Category> categoryRepo)
    {
        _categoryRepo = categoryRepo;
    }
    public async Task<IEnumerable<NameDto>> GetCategories()
    {
        var spec = new CategoryOrderByNameSpec();
        var data = await _categoryRepo.ListAsync(spec);

        return data.ToNameDtoList();
    }
}