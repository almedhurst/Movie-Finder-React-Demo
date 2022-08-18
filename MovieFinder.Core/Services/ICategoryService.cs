using MovieFinder.Core.Dtos;

namespace MovieFinder.Core.Services;

public interface ICategoryService
{
    Task<IEnumerable<NameDto>> GetCategories();
}