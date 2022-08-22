using Ardalis.Specification;
using MovieFinder.Core.Entities;

namespace MovieFinder.Infrastructure.Specifications;

public class UserFavouriteTitleByUserIdTitleIdSpec : Specification<UserFavouriteTitle>, ISingleResultSpecification
{
    public UserFavouriteTitleByUserIdTitleIdSpec(string userId, string titleId)
    {
        Query.Where(x => x.UserId == userId && x.TitleId == titleId);
    }
}