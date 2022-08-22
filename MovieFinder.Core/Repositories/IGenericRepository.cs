using Ardalis.Specification;
using MovieFinder.Core.Entities;

namespace MovieFinder.Core.Repositories;

public interface IGenericRepository<T> : IRepositoryBase<T> where T : BaseEntity
{
    
}