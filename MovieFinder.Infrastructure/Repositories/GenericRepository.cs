using Ardalis.Specification.EntityFrameworkCore;
using MovieFinder.Core.Entities;
using MovieFinder.Core.Repositories;
using MovieFinder.Infrastructure.Data;

namespace MovieFinder.Infrastructure.Repositories;

public class GenericRepository<T> : RepositoryBase<T>, IGenericRepository<T> where T : BaseEntity
{
    private readonly MovieContext _movieContext;

    public GenericRepository(MovieContext movieContext) : base(movieContext)
    {
        _movieContext = movieContext;
    }
}