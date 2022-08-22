using Ardalis.Specification;
using MovieFinder.Core.Entities;

namespace MovieFinder.Infrastructure.Specifications;

public class UserFavouriteTitlesByUserIdSpec : Specification<UserFavouriteTitle>
{
    public UserFavouriteTitlesByUserIdSpec(string userId)
    {
        Query.Where(x => x.UserId == userId);

    }
}