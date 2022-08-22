using Microsoft.AspNetCore.Identity;

namespace MovieFinder.Core.Entities;

public class User : IdentityUser
{
    public string GivenName { get; set; }
    public List<UserFavouriteTitle> UserFavouriteTitles { get; set; }
}