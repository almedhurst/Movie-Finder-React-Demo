using MovieFinder.Core.Entities;

namespace MovieFinder.Core.Services;

public interface ITokenService
{
    string GenerateToken(User user);
}